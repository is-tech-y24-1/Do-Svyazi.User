using System.Linq;
using Moq;

namespace CQRS.Tests.Extensions;

public static class MockExtensions
{
    private static void MockDbSet<T>(Mock mock, T[] items)
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
}