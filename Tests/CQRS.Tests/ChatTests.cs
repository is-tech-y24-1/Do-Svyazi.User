using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Do_Svyazi.User.Application.CQRS.Chats.Commands;
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
    public async Task AddUserToChat(IFixture fixture, [Greedy] MessengerUser user, [Greedy] GroupChat chat)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);

        var addUserToChatHandler = new AddUserToChat.Handler(dbContextMock.Object);
        var addUserToChatCommand = new AddUserToChat.Command(user.Id, chat.Id);

        await addUserToChatHandler.Handle(addUserToChatCommand, CancellationToken.None);

        ChatUser? expectedChatUser = fixture.Build<ChatUser>()
            .With(chatUser => chatUser.Chat, chat)
            .With(chatUser => chatUser.User, user)
            .With(chatUser => chatUser.ChatId, chat.Id)
            .With(chatUser => chatUser.MessengerUserId, user.Id)
            .Create();

        ChatUser gainChatUser = chat.Users.Single();

        gainChatUser.Should().BeEquivalentTo(expectedChatUser);
    }
    
    [Theory, AutoData]
    public async Task DeleteUserToChat([Greedy] MessengerUser user, [Greedy] GroupChat chat)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);
        
        var addUserToChatHandler = new AddUserToChat.Handler(dbContextMock.Object);
        var addUserToChatCommand = new AddUserToChat.Command(user.Id, chat.Id);
        await addUserToChatHandler.Handle(addUserToChatCommand, CancellationToken.None);
        chat.Users.Count.Should().Be(1);

        var deleteUserToChatHandler = new DeleteUserFromChat.Handler(dbContextMock.Object);
        var deleteUserToChatCommand = new DeleteUserFromChat.Command(user.Id, chat.Id);
        await deleteUserToChatHandler.Handle(deleteUserToChatCommand, CancellationToken.None);  
        chat.Users.Count.Should().Be(0);
    }
}