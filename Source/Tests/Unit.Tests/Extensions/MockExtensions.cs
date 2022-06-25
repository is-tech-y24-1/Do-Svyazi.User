using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using MockQueryable.Moq;
using Moq;

namespace CQRS.Tests.Extensions;

public static class MockExtensions
{
    public static void MockDbSet<T>(Mock mock, T[] items)
    {
        IQueryable<T> itemsQueryable = items.AsQueryable();

        mock.As<IQueryable<T>>()
            .Setup(x => x.Provider)
            .Returns(itemsQueryable.Provider);

        mock.As<IQueryable<T>>()
            .Setup(x => x.Expression)
            .Returns(itemsQueryable.Expression);

        mock.As<IQueryable<T>>()
            .Setup(x => x.ElementType)
            .Returns(itemsQueryable.ElementType);

        mock.As<IQueryable<T>>()
            .Setup(x => x.GetEnumerator())
            .Returns(itemsQueryable.GetEnumerator());
    }

    public static Mock<T> MockUserManager<T, TOut>(List<TOut>? users = null)
        where TOut : class
        where T : UserManager<TOut>
    {
        var mock = new Mock<T>(new Mock<IUserStore<TOut>>().Object, null, null, null, null, null, null, null, null);
        mock.Object.PasswordValidators.Add(new PasswordValidator<TOut>());
        
        if (users != null)
        {
            mock.Setup(x => x.CreateAsync(It.IsAny<TOut>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<TOut, string>((x, y) => users.Add(x));

            mock.Setup(x => x.Users).Returns(users.AsQueryable().BuildMock());
        }


        return mock;
    }

    public static Mock<T> MockRoleManager<T, TOut>()
        where TOut : class
        where T : RoleManager<TOut>
    {
        var mock = new Mock<T>(new Mock<IRoleStore<TOut>>().Object, new RoleValidator<TOut>[] { }, null, null, null);

        return mock;
    }
}