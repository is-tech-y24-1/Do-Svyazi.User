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
using Assert = NUnit.Framework.Assert;

namespace CQRS.Tests;

public class UserTests
{
    private readonly Fixture _fixture;

    public UserTests()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Xunit.Theory, AutoData]
    public async Task AddUser()
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Users);

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command("name1", "nickName1", "description1");
        Guid userId = await addUserHandler.Handle(addUserCommand, CancellationToken.None);

        var getUserHandler = new GetUser.Handler(dbContextMock.Object);
        var getUserCommand = new GetUser.Command(userId);
        MessengerUser user = await getUserHandler.Handle(getUserCommand, CancellationToken.None);

        Assert.AreEqual(user.Id, userId);
        Assert.AreEqual(user.Name, "name1");
        Assert.AreEqual(user.NickName, "nickName1");
        Assert.AreEqual(user.Description, "description1");
    }
    
    [Xunit.Theory, AutoData]
    public async Task DeleteUser()
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Users);

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command("name1", "nickName1", "description1");
        Guid userId = await addUserHandler.Handle(addUserCommand, CancellationToken.None);
        
        var deleteUserHandler = new DeleteUser.Handler(dbContextMock.Object);
        var deleteUserCommand = new DeleteUser.Command(userId);
        await deleteUserHandler.Handle(deleteUserCommand, CancellationToken.None);

        var getUserHandler = new GetUser.Handler(dbContextMock.Object);
        var getUserCommand = new GetUser.Command(userId);
        Assert.CatchAsync<Do_Svyazi_User_NotFoundException>(async () =>
        {
            await getUserHandler.Handle(getUserCommand, CancellationToken.None);
        });
    }
    
    [Xunit.Theory, AutoData]
    public async Task ChangeUserNameById()
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Users);

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command("name1", "nickName1", "description1");
        Guid userId = await addUserHandler.Handle(addUserCommand, CancellationToken.None);
        
        var changeUserNameHandler = new ChangeUserNameById.Handler(dbContextMock.Object);
        var changeUserNameCommand = new ChangeUserNameById.Command(userId, "name2");
        await changeUserNameHandler.Handle(changeUserNameCommand, CancellationToken.None);

        var getUserHandler = new GetUser.Handler(dbContextMock.Object);
        var getUserCommand = new GetUser.Command(userId);
        MessengerUser user = await getUserHandler.Handle(getUserCommand, CancellationToken.None);
        
        Assert.AreEqual(user.Id, userId);
        Assert.AreEqual(user.Name, "name2");
    }
}
