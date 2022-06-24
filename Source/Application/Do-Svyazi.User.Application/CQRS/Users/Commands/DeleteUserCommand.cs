using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record DeleteUserCommand(Guid userId)
    : IRequest;