using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record ChangeUserDescriptionByIdCommand(string userId, string description)
    : IRequest;