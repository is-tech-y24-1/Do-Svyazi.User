using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Users;
using EntityFrameworkCoreMock;
using FluentAssertions;

namespace CQRS.Tests;

public class ChatTests
{
    [Xunit.Theory, AutoData]
    public async Task AddGroupChat([Greedy] MessengerUser user, [Greedy] GroupChat chat)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats);
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);
        
        var addChatHandler = new AddGroupChat.Handler(dbContextMock.Object);
        var addChatCommand = new AddGroupChat.Command(user.Id, chat.Name, chat.Description);
        await addChatHandler.Handle(addChatCommand, CancellationToken.None);
    }

    [Xunit.Theory, AutoData]
    public async Task AddUserToChat([Greedy] MessengerUser user, [Greedy] GroupChat chat)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.Users);
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command(user.Name, user.NickName, user.Description);
        Guid userId = await addUserHandler.Handle(addUserCommand, CancellationToken.None);
        
        var addUserToChatHandler = new AddUserToChat.Handler(dbContextMock.Object);
        var addUserToChatCommand = new AddUserToChat.Command(userId, chat.Id);

        await addUserToChatHandler.Handle(addUserToChatCommand, CancellationToken.None);  
        chat.Users.Count.Should().Be(1);
    }
    
    [Xunit.Theory, AutoData]
    public async Task DeleteUserToChat([Greedy] MessengerUser user, [Greedy] GroupChat chat)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.Users);
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command(user.Name, user.NickName, user.Description);
        Guid userId = await addUserHandler.Handle(addUserCommand, CancellationToken.None);
        
        var addUserToChatHandler = new AddUserToChat.Handler(dbContextMock.Object);
        var addUserToChatCommand = new AddUserToChat.Command(userId, chat.Id);
        await addUserToChatHandler.Handle(addUserToChatCommand, CancellationToken.None);
        chat.Users.Count.Should().Be(1);

        var deleteUserToChatHandler = new DeleteUserToChat.Handler(dbContextMock.Object);
        var deleteUserToChatCommand = new DeleteUserToChat.Command(userId, chat.Id);
        await deleteUserToChatHandler.Handle(deleteUserToChatCommand, CancellationToken.None);  
        chat.Users.Count.Should().Be(0);
    }
}