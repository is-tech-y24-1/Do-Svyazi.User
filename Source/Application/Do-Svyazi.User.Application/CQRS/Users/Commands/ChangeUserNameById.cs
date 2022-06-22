using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record ChangeUserNameById(Guid userId, string name)
    : IRequest;