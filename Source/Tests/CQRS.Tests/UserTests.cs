using System;
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
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Users;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace CQRS.Tests;

public class UserTests
{
    [Theory, AutoData]
    public async Task AddUser_UserAdded([Frozen] IFixture fixture, MessengerUser messengerUser)
    {
        var userStore = new Mock<IUserStore<MessengerUser>>();
        var mgr = new Mock<UserManager<MessengerUser>>(userStore.Object, null, null, null, null, null, null, null, null);
        mgr.Object.PasswordValidators.Add(new PasswordValidator<MessengerUser>());
        var users = new List<MessengerUser>();

        var roleStore = new Mock<IRoleStore<MessageIdentityRole>>();
        var userManager = new RoleManager<MessageIdentityRole>(roleStore.Object, null, null, null, null);

        mgr.Setup(x => x.CreateAsync(It.IsAny<MessengerUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success).Callback<MessengerUser, string>((x, y) => users.Add(x));
        mgr.Setup(x => x.Users).Returns(users.AsQueryable());
        
        var usersCommandHandler = new AuthenticateCommandHandler(mgr.Object, userManager);

        RegisterModel registerModel = fixture.Build<RegisterModel>()
            .With(model => model.Email, messengerUser.Email)
            .With(model => model.Name, messengerUser.UserName)
            .With(model => model.UserName, messengerUser.UserName)
            .With(model => model.PhoneNumber, messengerUser.PhoneNumber)
            .Create();
        
        var addUserCommand = new RegisterCommand(registerModel);
        await usersCommandHandler.Handle(addUserCommand, CancellationToken.None);

        MessengerUser gainMessengerUser = mgr.Object.Users.Single();

        gainMessengerUser.UserName.Should().Be(registerModel.UserName);
        gainMessengerUser.Name.Should().Be(registerModel.Name);
        gainMessengerUser.Email.Should().Be(registerModel.Email);
        gainMessengerUser.PhoneNumber.Should().Be(registerModel.PhoneNumber);
    }

    [Theory, AutoData]
    public async Task ChangeUserNameById_UserNameChanged(
        [Greedy] MessengerUser user,
        string newName)
    {
        var store = new Mock<IUserStore<MessengerUser>>();
        var mgr = new Mock<UserManager<MessengerUser>>(store.Object, null, null, null, null, null, null, null, null);
        var usersQueryMock = new[] {user}.AsQueryable().BuildMock();

        mgr.Setup(x => x.Users).Returns(usersQueryMock);
        mgr.Setup( userManager => userManager.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(user);

        var usersCommandHandler = new UsersCommandHandler(mgr.Object);
        var changeUserNameCommand = new ChangeUserNameByIdCommand($"{user.Id}", newName);
        await usersCommandHandler.Handle(changeUserNameCommand, CancellationToken.None);

        MessengerUser gainMessengerUser = mgr.Object.Users.Single();

        gainMessengerUser.Name.Should().Be(newName);
    }
}