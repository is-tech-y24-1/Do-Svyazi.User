using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public record GetAllChatsIdsByUserId(Guid userId)
    : IRequest<IReadOnlyList<Guid>>;