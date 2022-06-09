using System.Threading;
using AutoFixture;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.DataAccess;
using Moq;
using Xunit;

namespace CQRS.Tests;


public class UserTests
{
    [Fact, AutoFixture.Xunit2.AutoData]
    public async void Test1()
    {
        var fixture = new Fixture();
        var db = new Mock<DoSvaziDbContext>();
        var addUserHandler = new AddUser.Handler(db.Object);
        var addUserCommand = new AddUser.Command("name1", "nickName1", "description1");
        
        await addUserHandler.Handle(addUserCommand, CancellationToken.None);
    }
}