using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record ChangeUserDescriptionByIdCommand(Guid userId, string description)
    : IRequest;