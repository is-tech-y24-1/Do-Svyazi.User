using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Chats.Handlers;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Handlers;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Users;
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
    public async Task AddUsersToChat_CheckUserHaveChat_CheckChatHAveUsers()
    {
        var messengerUser1 = new MessengerUser("name1", "nickname1", "des1");
        var messengerUser2 = new MessengerUser("name2", "nickname2", "des2");
        
        var mapper = GenerateMapper();
        var chatsCommandHandler = new ChatsCommandHandler(_context);
        var usersCommandHandler = new UsersCommandHandler(_context);
        var usersQueryHandler = new UsersQueryHandler(_context, mapper);

        var addUser = new AddUser(messengerUser1.Name, messengerUser1.NickName, messengerUser1.Description);
        Guid userId1 = await usersCommandHandler.Handle(addUser, CancellationToken.None);
        
        addUser = new AddUser(messengerUser2.Name, messengerUser2.NickName, messengerUser2.Description);
        Guid userId2 = await usersCommandHandler.Handle(addUser, CancellationToken.None);
        
        var addGroupChat = new AddGroupChat(userId1, "chat1", "description1");
        Guid chatGroupId = await chatsCommandHandler.Handle(addGroupChat, CancellationToken.None);
        
        var addUserToChatCommand = new AddUserToChat(userId2, chatGroupId);
        await chatsCommandHandler.Handle(addUserToChatCommand, CancellationToken.None);

        var getUser = new GetUser(userId1);
        MessengerUser user1 = await usersQueryHandler.Handle(getUser, CancellationToken.None);

        getUser = new GetUser(userId2);
        MessengerUser user2 = await usersQueryHandler.Handle(getUser, CancellationToken.None);

        user1.Chats.Should().HaveCount(1);
        user2.Chats.Should().HaveCount(1);
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