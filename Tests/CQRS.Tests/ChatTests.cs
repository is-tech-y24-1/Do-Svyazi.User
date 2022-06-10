using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Users;
using EntityFrameworkCoreMock;
using Assert = NUnit.Framework.Assert;

namespace CQRS.Tests;

public class ChatTests
{
    private readonly Fixture _fixture;

    public ChatTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Xunit.Theory, AutoData]
    public async Task AddGroupChat()
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats);
        dbContextMock.CreateDbSetMock(x => x.Users);
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);
        
        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command("name1", "nickName1", "description1");
        Guid userId = await addUserHandler.Handle(addUserCommand, CancellationToken.None);
        
        var addChatHandler = new AddGroupChat.Handler(dbContextMock.Object);
        var addChatCommand = new AddGroupChat.Command(userId, "ChatName", "ChatDescription");
        var chat = await addChatHandler.Handle(addChatCommand, CancellationToken.None);

        Assert.AreEqual(chat.CreatorId, userId);
    }

    [Xunit.Theory, AutoData]
    public async Task AddUserToChat()
    {
        _fixture.Customizations.Add(
            new TypeRelay(
                typeof(Chat),
                typeof(GroupChat)));
        
        var chat = _fixture
            .Build<GroupChat>()
            .With(x => x.Users, new List<ChatUser>())
            .Create();
        
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.Users);
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command("name1", "nickName1", "description1");
        
        Guid userId = await addUserHandler.Handle(addUserCommand, CancellationToken.None);
        
        var addUserToChatHandler = new AddUserToChat.Handler(dbContextMock.Object);
        var addUserToChatCommand = new AddUserToChat.Command(userId, chat.Id);

        await addUserToChatHandler.Handle(addUserToChatCommand, CancellationToken.None);  
        Assert.AreEqual(chat.Users.Count, 1);
    }
    
    [Xunit.Theory, AutoData]
    public async Task DeleteUserToChat(MessengerUser user)
    {
        _fixture.Customizations.Add(
            new TypeRelay(
                typeof(Chat),
                typeof(GroupChat)));
        
        GroupChat? chat = _fixture
            .Build<GroupChat>()
            .With(x => x.Users, new List<ChatUser>())
            .Create();
        
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.Users);
        dbContextMock.CreateDbSetMock(x => x.ChatUsers);

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command("name1", "nickName1", "description1");
        Guid userId = await addUserHandler.Handle(addUserCommand, CancellationToken.None);
        
        var addUserToChatHandler = new AddUserToChat.Handler(dbContextMock.Object);
        var addUserToChatCommand = new AddUserToChat.Command(userId, chat.Id);
        await addUserToChatHandler.Handle(addUserToChatCommand, CancellationToken.None);
        Assert.AreEqual(chat.Users.Count, 1);

        var deleteUserToChatHandler = new DeleteUserToChat.Handler(dbContextMock.Object);
        var deleteUserToChatCommand = new DeleteUserToChat.Command(userId, chat.Id);
        await deleteUserToChatHandler.Handle(deleteUserToChatCommand, CancellationToken.None);  
        Assert.AreEqual(chat.Users.Count, 0);
    }
}