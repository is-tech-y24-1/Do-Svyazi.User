using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record AddUserCommand(string name, string nickName, string description)
    : IRequest<Guid>;