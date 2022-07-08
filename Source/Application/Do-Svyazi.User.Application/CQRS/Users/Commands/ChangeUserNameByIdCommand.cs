using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record ChangeUserNameByIdCommand(Guid userId, string name)
    : IRequest;