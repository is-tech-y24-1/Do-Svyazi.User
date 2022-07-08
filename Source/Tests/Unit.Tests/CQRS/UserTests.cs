using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Do_Svyazi.User.Application.CQRS.Authenticate.Commands;
using Do_Svyazi.User.Application.CQRS.Authenticate.Handlers;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.CQRS.Users.Handlers;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;
using static CQRS.Tests.Extensions.MockExtensions;

namespace CQRS.Tests.CQRS;

public class UserTests
{
    [Theory, AutoData]
    public async Task AddUser_UserAdded([Frozen] IFixture fixture, MessengerUser messengerUser)
    {
        var users = new List<MessengerUser>();

        var mockUserManager = MockUserManager<UserManager<MessengerUser>, MessengerUser>(users);
        var mockRoleManager = MockRoleManager<RoleManager<MessageIdentityRole>, MessageIdentityRole>();

        var usersCommandHandler = new AuthenticateCommandHandler(mockUserManager.Object, mockRoleManager.Object);

        RegisterModel registerModel = fixture.Build<RegisterModel>()
            .With(model => model.Email, messengerUser.Email)
            .With(model => model.Name, messengerUser.UserName)
            .With(model => model.UserName, messengerUser.UserName)
            .With(model => model.PhoneNumber, messengerUser.PhoneNumber)
            .Create();
        
        var addUserCommand = new RegisterCommand(registerModel);
        await usersCommandHandler.Handle(addUserCommand, CancellationToken.None);

        MessengerUser gainMessengerUser = mockUserManager.Object.Users.Single();

        gainMessengerUser.UserName.Should().Be(registerModel.UserName);
        gainMessengerUser.Name.Should().Be(registerModel.Name);
        gainMessengerUser.Email.Should().Be(registerModel.Email);
        gainMessengerUser.PhoneNumber.Should().Be(registerModel.PhoneNumber);
    }

    [Theory, AutoData]
    public async Task ChangeUserNameById_UserNameChanged(MessengerUser user, string newName)
    {
        var users = new List<MessengerUser> {user};

        var mockUserManager = MockUserManager<UserManager<MessengerUser>, MessengerUser>(users);
        mockUserManager.Setup( userManager => userManager.FindByIdAsync($"{user.Id}")).ReturnsAsync(user);

        var usersCommandHandler = new UsersCommandHandler(mockUserManager.Object);
        var changeUserNameCommand = new ChangeUserNameByIdCommand(user.Id, newName);
        await usersCommandHandler.Handle(changeUserNameCommand, CancellationToken.None);

        MessengerUser gainMessengerUser = mockUserManager.Object.Users.Single();

        gainMessengerUser.Name.Should().Be(newName);
    }
}