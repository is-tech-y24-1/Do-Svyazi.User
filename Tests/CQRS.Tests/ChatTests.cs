using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Xunit2;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.DataAccess;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Users;
using EntityFrameworkCoreMock;
using Moq;
using Xunit;

namespace CQRS.Tests;

public class UserTests
{
    [Theory, AutoData]
    public async Task Test1(
        GroupChat chat,
        MessengerUser user)
    {
        var dbContextMock = new DbContextMock<DoSvaziDbContext>();
        dbContextMock.CreateDbSetMock(x => x.Chats, new[] {chat});
        dbContextMock.CreateDbSetMock(x => x.Users, new[] {user});

        var addUserHandler = new AddUser.Handler(dbContextMock.Object);
        var addUserCommand = new AddUser.Command("name1", "nickName1", "description1");

        await addUserHandler.Handle(addUserCommand, CancellationToken.None);
    }
}