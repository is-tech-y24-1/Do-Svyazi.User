using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record SetUserNickNameByIdCommand(Guid userId, string userName)
    : IRequest;