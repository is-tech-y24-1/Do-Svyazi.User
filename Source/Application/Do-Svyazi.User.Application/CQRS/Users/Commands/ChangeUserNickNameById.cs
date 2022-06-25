using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record SetUserNickNameByIdCommand(string userId, string nickName)
    : IRequest;