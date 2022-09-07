namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

using MediatR;

public record GetUserIdsByChatId(Guid chatId)
    : IRequest<IReadOnlyCollection<Guid>>;