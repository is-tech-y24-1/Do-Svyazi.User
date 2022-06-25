using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record ChangeUserNameByIdCommand(string userId, string name)
    : IRequest;