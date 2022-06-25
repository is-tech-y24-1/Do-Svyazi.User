using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Do_Svyazi.User.Application.CQRS.Authenticate.Commands;
using Do_Svyazi.User.Application.CQRS.Authenticate.Handlers;
using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Chats.Handlers;
using Do_Svyazi.User.Application.CQRS.Chats.Queries;
using Do_Svyazi.User.Application.CQRS.Users.Handlers;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using Do_Svyazi.User.Dtos.Mapping;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Integration.Tests;

[TestFixture]
public class IntegrationTests : IDisposable
{
    private IMapper _mapper;
    private DoSvaziDbContext _context;
    private ApplicationDbContext _identityContext;
    private UserManager<MessengerUser> _userManager;
    private RoleManager<MessageIdentityRole> _roleManager;

    [OneTimeSetUp]
    public void Setup()
    {
        _mapper = new MapperConfiguration(c =>
        {
            c.AddProfile<MappingProfile>();
        }).CreateMapper();

        _context = new DoSvaziDbContext(
            new DbContextOptionsBuilder<DoSvaziDbContext>()
                .UseInMemoryDatabase("test_db")
                .Options);

        _identityContext = new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("user_identity_test_db")
                .Options);

        _userManager = new UserManager<MessengerUser>(
            new UserStore<MessengerUser, MessageIdentityRole, ApplicationDbContext, Guid>(_identityContext), null,
            new PasswordHasher<MessengerUser>(), null, null, null, null, null, null);

        _roleManager = new RoleManager<MessageIdentityRole>(
            new RoleStore<MessageIdentityRole, ApplicationDbContext, Guid>(_identityContext), null, null, null, null);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _identityContext.Database.EnsureDeleted();
    }

    [Test]
    public async Task AddUsersToChat_CheckUserHaveChat_CheckChatHaveUsers()
    {
        var chatsCommandHandler = new ChatsCommandHandler(_userManager, _context);
        var authCommandHandler = new AuthenticateCommandHandler(_userManager, _roleManager);
        var usersQueryHandler = new UsersQueryHandler(_userManager, _context, _mapper);
        var chatsQueryHandler = new ChatsQueryHandler(_context, _mapper);

        RegisterModel userModel1 = CreateRegisterModel("name1", "nickname1", "email1", "phoneNumber1");
        RegisterModel userModel2 = CreateRegisterModel("name2", "nickname2", "email2", "phoneNumber2");

        var userId1 = await authCommandHandler.Handle(new RegisterCommand(userModel1), CancellationToken.None);
        var userId2 = await authCommandHandler.Handle(new RegisterCommand(userModel2), CancellationToken.None);

        Guid chatGroupId = await chatsCommandHandler.Handle(new AddGroupChatCommand(userId1, "chat1", "description1"), CancellationToken.None);
        await chatsCommandHandler.Handle(new AddUserToChatCommand(userId2, chatGroupId), CancellationToken.None);

        var userChats1 =
            await usersQueryHandler.Handle(new GetAllChatsIdsByUserIdQuery(userId1), CancellationToken.None);
        var userChats2 =
            await usersQueryHandler.Handle(new GetAllChatsIdsByUserIdQuery(userId2), CancellationToken.None);
        MessengerChatDto chat =
            await chatsQueryHandler.Handle(new GetChatByIdQuery(chatGroupId), CancellationToken.None);

        chat.Users.Should()
            .Contain(userId1)
            .And.Contain(userId2)
            .And.HaveCount(2);

        userChats1.Should().Contain(chatGroupId).And.HaveCount(1);
        userChats2.Should().Contain(chatGroupId).And.HaveCount(1);
    }

    [Test]
    public async Task DeleteUserFromChat_CheckUserNotHaveChat()
    {
        var chatsCommandHandler = new ChatsCommandHandler(_userManager, _context);
        var authCommandHandler = new AuthenticateCommandHandler(_userManager, _roleManager);
        var usersQueryHandler = new UsersQueryHandler(_userManager, _context, _mapper);
        var chatsQueryHandler = new ChatsQueryHandler(_context, _mapper);

        RegisterModel userModel1 = CreateRegisterModel("name1", "nickname1", "email1", "phoneNumber1");
        RegisterModel userModel2 = CreateRegisterModel("name2", "nickname2", "email2", "phoneNumber2");
        RegisterModel userModel3 = CreateRegisterModel("name3", "nickname3", "email3", "phoneNumber3");


        Guid userId1 = await authCommandHandler.Handle(new RegisterCommand(userModel1), CancellationToken.None);
        Guid userId2 = await authCommandHandler.Handle(new RegisterCommand(userModel2), CancellationToken.None);
        Guid userId3 = await authCommandHandler.Handle(new RegisterCommand(userModel3), CancellationToken.None);

        var addGroupChat = new AddGroupChatCommand(userId1, "chat1", "description1");
        Guid chatGroupId = await chatsCommandHandler.Handle(addGroupChat, CancellationToken.None);

        var addUserToChatCommand = new AddUserToChatCommand(userId2, chatGroupId);
        await chatsCommandHandler.Handle(addUserToChatCommand, CancellationToken.None);
        addUserToChatCommand = new AddUserToChatCommand(userId3, chatGroupId);
        await chatsCommandHandler.Handle(addUserToChatCommand, CancellationToken.None);

        var userChats1 =
            await usersQueryHandler.Handle(new GetAllChatsIdsByUserIdQuery(userId1), CancellationToken.None);
        var userChats2 =
            await usersQueryHandler.Handle(new GetAllChatsIdsByUserIdQuery(userId2), CancellationToken.None);
        var userChats3 =
            await usersQueryHandler.Handle(new GetAllChatsIdsByUserIdQuery(userId3), CancellationToken.None);

        userChats1.Should().Contain(chatGroupId).And.HaveCount(1);
        userChats2.Should().Contain(chatGroupId).And.HaveCount(1);
        userChats3.Should().Contain(chatGroupId).And.HaveCount(1);

        await chatsCommandHandler.Handle(new DeleteUserFromChatCommand(userId3, chatGroupId), CancellationToken.None);
        userChats3 = await usersQueryHandler.Handle(new GetAllChatsIdsByUserIdQuery(userId3), CancellationToken.None);

        MessengerChatDto chat =
            await chatsQueryHandler.Handle(new GetChatByIdQuery(chatGroupId), CancellationToken.None);

        chat.Users.Should().NotContain(userId3).And.HaveCount(2);
        userChats3.Should().NotContain(chatGroupId).And.HaveCount(0);
    }

    public void Dispose()
    {
        _context.Dispose();
        _identityContext.Dispose();
    }

    private RegisterModel CreateRegisterModel(string userName, string name, string email, string phoneNumber) => new()
    {
        UserName = userName,
        Name = name,
        Email = email,
        Password = $"Test??111{Guid.NewGuid()}",
        PhoneNumber = phoneNumber,
    };
}