using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public record AddGroupChat(Guid adminId, string name, string description)
    : IRequest<Guid>;