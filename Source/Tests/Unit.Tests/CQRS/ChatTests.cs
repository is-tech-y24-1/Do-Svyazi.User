using System.Collections.Generic;
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
using Microsoft.AspNetCore.Identity;
using Xunit;
using static CQRS.Tests.Extensions.MockExtensions;

namespace CQRS.Tests.CQRS;

public class ChatTests
{
    [Theory, AutoData]
    public async Task AddUserToChat_UserAdded(
        IFixture fixture,
        MessengerUser user,
        [Greedy] GroupChat chat)
    {
        var users = new List<MessengerUser> {user};

        var mockUserManager = MockUserManager<UserManager<MessengerUser>, MessengerUser>(users);
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();

        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);

        var addUserToChatHandler = new ChatsCommandHandler(mockUserManager.Object, dbContextMock.Object);
        var addUserToChatCommand = new AddUserToChatCommand(user.Id, chat.Id);

        // as soon as we have chat creator
        chat.Users.Should().HaveCount(1);

        await addUserToChatHandler.Handle(addUserToChatCommand, CancellationToken.None);

        ChatUser expectedChatUser = fixture.Build<ChatUser>()
            .With(chatUser => chatUser.Chat, chat)
            // .With(chatUser => chatUser.User, user)
            .With(chatUser => chatUser.ChatId, chat.Id)
            .With(chatUser => chatUser.MessengerUserId, user.Id)
            .Create();

        chat.Users.Should().Contain(expectedChatUser).And.HaveCount(2);
    }

    [Theory, AutoData]
    public async Task DeleteUserFromChat_UsersListEmpty(
        MessengerUser user,
        [Greedy] GroupChat chat)
    {
        var users = new List<MessengerUser> {user};

        var mockUserManager = MockUserManager<UserManager<MessengerUser>, MessengerUser>(users);
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);

        var chatsCommandHandler = new ChatsCommandHandler(mockUserManager.Object, dbContextMock.Object);

        var addUserToChatCommand = new AddUserToChatCommand(user.Id, chat.Id);
        await chatsCommandHandler.Handle(addUserToChatCommand, CancellationToken.None);

        // as soon as we have chat creator
        chat.Users.Should().HaveCount(2);

        var deleteUserFromChatCommand = new DeleteUserFromChatCommand(user.Id, chat.Id);
        await chatsCommandHandler.Handle(deleteUserFromChatCommand, CancellationToken.None);

        chat.Users.Should().HaveCount(1);
    }
}