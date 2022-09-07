using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record DeleteUser(Guid userId)
    : IRequest;