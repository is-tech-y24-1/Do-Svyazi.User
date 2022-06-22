using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public record ChangeUserDescriptionById(Guid userId, string description)
    : IRequest;