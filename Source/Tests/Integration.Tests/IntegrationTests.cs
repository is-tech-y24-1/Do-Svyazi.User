using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Chats.Handlers;
using Do_Svyazi.User.Application.CQRS.Chats.Queries;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Handlers;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using Do_Svyazi.User.Dtos.Mapping;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Integration.Tests;

[TestFixture]
public class IntegrationTests : IDisposable
{
    private DoSvaziDbContext _context;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DoSvaziDbContext>()
            .UseInMemoryDatabase("test_db")
            .Options;
        
        _context = new DoSvaziDbContext(options);
    }
    
    [TearDown]
    public void TearDown() => _context.Database.EnsureDeleted();

    [Test]
    public async Task AddUsersToChat_CheckUserHaveChat_CheckChatHaveUsers()
    {
        var messengerUser1 = new MessengerUser("name1", "nickname1", "description1");
        var messengerUser2 = new MessengerUser("name2", "nickname2", "description2");
        
        var mapper = GenerateMapper();
        var chatsCommandHandler = new ChatsCommandHandler(_context);
        var usersCommandHandler = new UsersCommandHandler(_context);
        var usersQueryHandler = new UsersQueryHandler(_context, mapper);
        var chatsQueryHandler = new ChatsQueryHandler(_context, mapper);

        var addUser = new AddUser(messengerUser1.Name, messengerUser1.NickName, messengerUser1.Description);
        Guid userId1 = await usersCommandHandler.Handle(addUser, CancellationToken.None);
        
        addUser = new AddUser(messengerUser2.Name, messengerUser2.NickName, messengerUser2.Description);
        Guid userId2 = await usersCommandHandler.Handle(addUser, CancellationToken.None);
        
        var addGroupChat = new AddGroupChat(userId1, "chat1", "description1");
        Guid chatGroupId = await chatsCommandHandler.Handle(addGroupChat, CancellationToken.None);
        
        var addUserToChatCommand = new AddUserToChat(userId2, chatGroupId);
        await chatsCommandHandler.Handle(addUserToChatCommand, CancellationToken.None);

        var getAllChatsIdsByUserId = new GetAllChatsIdsByUserId(userId1);
        var userChats1 = await usersQueryHandler.Handle(getAllChatsIdsByUserId, CancellationToken.None);

        getAllChatsIdsByUserId = new GetAllChatsIdsByUserId(userId2);
        var userChats2 = await usersQueryHandler.Handle(getAllChatsIdsByUserId, CancellationToken.None);
        var getChatById = new GetChatById(chatGroupId);
        MessengerChatDto chat = await chatsQueryHandler.Handle(getChatById, CancellationToken.None);

        chat.Users.Should().Contain(userId1)
            .And.Contain(userId2)
            .And.HaveCount(2);
        userChats1.Should().Contain(chatGroupId).And.HaveCount(1);
        userChats2.Should().Contain(chatGroupId).And.HaveCount(1);
    }
    
    [Test]
    public async Task DeleteUserFromChat_CheckUserNotHaveChat()
    {
        var messengerUser1 = new MessengerUser("name1", "nickname1", "description1");
        var messengerUser2 = new MessengerUser("name2", "nickname2", "description2");
        var messengerUser3 = new MessengerUser("name3", "nickname3", "description3");
        
        var mapper = GenerateMapper();
        var chatsCommandHandler = new ChatsCommandHandler(_context);
        var usersCommandHandler = new UsersCommandHandler(_context);
        var usersQueryHandler = new UsersQueryHandler(_context, mapper);
        var chatsQueryHandler = new ChatsQueryHandler(_context, mapper);

        var addUser = new AddUser(messengerUser1.Name, messengerUser1.NickName, messengerUser1.Description);
        Guid userId1 = await usersCommandHandler.Handle(addUser, CancellationToken.None);
        addUser = new AddUser(messengerUser2.Name, messengerUser2.NickName, messengerUser2.Description);
        Guid userId2 = await usersCommandHandler.Handle(addUser, CancellationToken.None);
        addUser = new AddUser(messengerUser3.Name, messengerUser3.NickName, messengerUser3.Description);
        Guid userId3 = await usersCommandHandler.Handle(addUser, CancellationToken.None);
        
        var addGroupChat = new AddGroupChat(userId1, "chat1", "description1");
        Guid chatGroupId = await chatsCommandHandler.Handle(addGroupChat, CancellationToken.None);
        
        var addUserToChatCommand = new AddUserToChat(userId2, chatGroupId);
        await chatsCommandHandler.Handle(addUserToChatCommand, CancellationToken.None);
        addUserToChatCommand = new AddUserToChat(userId3, chatGroupId);
        await chatsCommandHandler.Handle(addUserToChatCommand, CancellationToken.None);

        var getAllChatsIdsByUserId = new GetAllChatsIdsByUserId(userId1);
        var userChats1 = await usersQueryHandler.Handle(getAllChatsIdsByUserId, CancellationToken.None);

        getAllChatsIdsByUserId = new GetAllChatsIdsByUserId(userId2);
        var userChats2 = await usersQueryHandler.Handle(getAllChatsIdsByUserId, CancellationToken.None);
        
        getAllChatsIdsByUserId = new GetAllChatsIdsByUserId(userId3);
        var userChats3 = await usersQueryHandler.Handle(getAllChatsIdsByUserId, CancellationToken.None);

        userChats1.Should().Contain(chatGroupId).And.HaveCount(1);
        userChats2.Should().Contain(chatGroupId).And.HaveCount(1);
        userChats3.Should().Contain(chatGroupId).And.HaveCount(1);

        var deleteUserFromChat = new DeleteUserFromChat(userId3, chatGroupId);
        await chatsCommandHandler.Handle(deleteUserFromChat, CancellationToken.None);
        getAllChatsIdsByUserId = new GetAllChatsIdsByUserId(userId3);
        userChats3 = await usersQueryHandler.Handle(getAllChatsIdsByUserId, CancellationToken.None);
        
        var getChatById = new GetChatById(chatGroupId);
        MessengerChatDto chat = await chatsQueryHandler.Handle(getChatById, CancellationToken.None);

        chat.Users.Should().NotContain(userId3).And.HaveCount(2);
        userChats3.Should().NotContain(chatGroupId).And.HaveCount(0);
    }
    
    public static IMapper GenerateMapper()
    {
        var mapperConfig = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        });
        
        return mapperConfig.CreateMapper();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}