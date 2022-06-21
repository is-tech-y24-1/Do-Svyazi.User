using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Users;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Xunit;

namespace CQRS.Tests;

public class UserTests
{
    [Theory, AutoData]
    public async Task AddUser_UserAdded([Greedy] MessengerUser user)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>(); 
        dbContextMock.CreateDbSetMock(x => x.Users, Array.Empty<MessengerUser>());

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        
        var addUserCommand = new AddUser { Name = user.Name, NickName = user.NickName, Description = user.Description };
        await addUserHandler.Handle(addUserCommand, CancellationToken.None);

        MessengerUser gainMessengerUser = dbContextMock.Object.Users.Single();
        
        gainMessengerUser.Name.Should().Be(user.Name);
        gainMessengerUser.NickName.Should().Be(user.NickName);
        gainMessengerUser.Description.Should().Be(user.Description);
    }

    [Theory, AutoData]
    public async Task ChangeUserNameById_UserNameChanged(
        [Greedy] MessengerUser user,
        string newName)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});

        var changeUserNameHandler = new ChangeUserNameById.Handler(dbContextMock.Object);

        var changeUserNameCommand = new ChangeUserNameById { UserId = user.Id, Name = newName };
        await changeUserNameHandler.Handle(changeUserNameCommand, CancellationToken.None);

        MessengerUser gainMessengerUser = dbContextMock.Object.Users.Single();

        gainMessengerUser.Name.Should().Be(newName);
    }
}
