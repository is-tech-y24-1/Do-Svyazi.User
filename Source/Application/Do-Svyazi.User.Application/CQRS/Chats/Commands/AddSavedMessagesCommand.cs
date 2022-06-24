using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public record AddSavedMessagesCommand(Guid userId, string name, string description)
    : IRequest<Guid>;