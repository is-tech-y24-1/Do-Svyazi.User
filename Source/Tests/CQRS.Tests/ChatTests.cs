using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Chats.Handlers;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Users;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Xunit;

namespace CQRS.Tests;

public class ChatTests
{
    [Theory, AutoData]
    public async Task AddUserToChat_UserAdded(
        IFixture fixture,
        [Greedy] MessengerUser user,
        [Greedy] GroupChat chat)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();

        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);

        var addUserToChatHandler = new ChatsCommandHandler(dbContextMock.Object);
        var addUserToChatCommand = new AddUserToChatCommand(user.Id, chat.Id);

        chat.Users.Should().HaveCount(0);

        await addUserToChatHandler.Handle(addUserToChatCommand, CancellationToken.None);

        ChatUser expectedChatUser = fixture.Build<ChatUser>()
            .With(chatUser => chatUser.Chat, chat)
            .With(chatUser => chatUser.User, user)
            .With(chatUser => chatUser.ChatId, chat.Id)
            .With(chatUser => chatUser.MessengerUserId, user.Id)
            .Create();

        chat.Users.Should().HaveCount(1);
        ChatUser gainChatUser = chat.Users.Single();
        gainChatUser.Should().BeEquivalentTo(expectedChatUser);
    }

    [Theory, AutoData]
    public async Task DeleteUserFromChat_UsersListEmpty(
        [Greedy] MessengerUser user,
        [Greedy] GroupChat chat)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();

        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);

        var chatsCommandHandler = new ChatsCommandHandler(dbContextMock.Object);

        var addUserToChatCommand = new AddUserToChatCommand(user.Id, chat.Id);
        await chatsCommandHandler.Handle(addUserToChatCommand, CancellationToken.None);

        chat.Users.Should().HaveCount(1);

        var deleteUserToChatCommand = new DeleteUserFromChatCommand(user.Id, chat.Id);
        await chatsCommandHandler.Handle(deleteUserToChatCommand, CancellationToken.None);

        chat.Users.Should().HaveCount(0);
    }
}