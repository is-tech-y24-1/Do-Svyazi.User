using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record SetUserNickNameById(Guid userId, string nickName)
    : IRequest;