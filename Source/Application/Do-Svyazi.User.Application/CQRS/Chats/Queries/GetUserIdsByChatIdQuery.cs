namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

using MediatR;

public record GetUserIdsByChatIdQuery(Guid chatId)
    : IRequest<IReadOnlyCollection<Guid>>;