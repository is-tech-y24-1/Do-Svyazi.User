using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using EntityFrameworkCoreMock;
using FluentAssertions;

namespace CQRS.Tests;

public class UserTests
{
    [Xunit.Theory, AutoData]
    public async Task AddUser(IFixture fixture, [Greedy] MessengerUser user)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Users);

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command(user.Name, user.NickName, user.Description);
        await addUserHandler.Handle(addUserCommand, CancellationToken.None);

        MessengerUser gainMessengerUser = dbContextMock.Object.Users.Single();
        
        gainMessengerUser.Name.Should().Be(user.Name);
        gainMessengerUser.NickName.Should().Be(user.NickName);
        gainMessengerUser.Description.Should().Be(user.Description);
    }
    
    [Xunit.Theory, AutoData]
    public async Task ChangeUserNameById([Greedy] MessengerUser user, string newName)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});

        var changeUserNameHandler = new ChangeUserNameById.Handler(dbContextMock.Object);
        var changeUserNameCommand = new ChangeUserNameById.Command(user.Id, newName);
        await changeUserNameHandler.Handle(changeUserNameCommand, CancellationToken.None);

        MessengerUser gainMessengerUser = dbContextMock.Object.Users.Single();

        gainMessengerUser.Name.Should().Be(newName);
    }
}
