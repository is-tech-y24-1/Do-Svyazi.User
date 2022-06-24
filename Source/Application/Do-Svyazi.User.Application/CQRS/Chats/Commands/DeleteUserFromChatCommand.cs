using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public record DeleteUserFromChatCommand(Guid userId, Guid chatId)
    : IRequest;