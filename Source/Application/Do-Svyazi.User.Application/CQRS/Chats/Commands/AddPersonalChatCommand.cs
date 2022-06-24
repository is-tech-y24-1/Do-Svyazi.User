using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public record AddPersonalChatCommand(Guid firstUserId, Guid secondUserId, string name, string description)
    : IRequest<Guid>;