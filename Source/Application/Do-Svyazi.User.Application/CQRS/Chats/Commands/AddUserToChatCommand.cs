using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public record AddUserToChatCommand(Guid userId, Guid chatId)
    : IRequest<Unit>;