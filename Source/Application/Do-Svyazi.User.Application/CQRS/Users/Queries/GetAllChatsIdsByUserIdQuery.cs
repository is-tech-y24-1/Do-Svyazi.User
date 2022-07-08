using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public record GetAllChatsIdsByUserIdQuery(Guid userId)
    : IRequest<IReadOnlyList<Guid>>;