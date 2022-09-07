using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public record AddPersonalChat(Guid firstUserId, Guid secondUserId, string name, string description)
    : IRequest<Guid>;