using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public record DeleteUserFromChat(Guid userId, Guid chatId)
    : IRequest;