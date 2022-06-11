using System;
using System.Threading;
using System.Threading.Tasks;
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
    public async Task AddUser([Greedy] MessengerUser user)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Users);

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command(user.Name, user.NickName, user.Description);
        Guid userId = await addUserHandler.Handle(addUserCommand, CancellationToken.None);

        user.Name.Should().Be(user.Name);
        user.NickName.Should().Be(user.NickName);
        user.Description.Should().Be(user.Description);
    }
    
    [Xunit.Theory, AutoData]
    public async Task DeleteUser([Greedy] MessengerUser user)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});
        
        var deleteUserHandler = new DeleteUser.Handler(dbContextMock.Object);
        var deleteUserCommand = new DeleteUser.Command(user.Id);
        await deleteUserHandler.Handle(deleteUserCommand, CancellationToken.None);

        var getUserHandler = new GetUser.Handler(dbContextMock.Object);
        var getUserCommand = new GetUser.Command(user.Id);

        Func<Task> f = async () => await getUserHandler.Handle(getUserCommand, CancellationToken.None);
        await f.Should().ThrowAsync<Do_Svyazi_User_NotFoundException>();
    }
    
    [Xunit.Theory, AutoData]
    public async Task ChangeUserNameById([Greedy] MessengerUser user, string newName)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});

        var changeUserNameHandler = new ChangeUserNameById.Handler(dbContextMock.Object);
        var changeUserNameCommand = new ChangeUserNameById.Command(user.Id, newName);
        await changeUserNameHandler.Handle(changeUserNameCommand, CancellationToken.None);

        var getUserHandler = new GetUser.Handler(dbContextMock.Object);
        var getUserCommand = new GetUser.Command(user.Id);
        MessengerUser u = await getUserHandler.Handle(getUserCommand, CancellationToken.None);

        user.Id.Should().Be(u.Id);
        u.Name.Should().Be(newName);
    }
}
