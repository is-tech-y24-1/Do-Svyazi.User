using Do_Svyazi.User.Dtos.Users;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

using MediatR;

public record GetUsersByChatIdQuery(Guid chatId)
    : IRequest<IReadOnlyCollection<ChatUserDto>>;