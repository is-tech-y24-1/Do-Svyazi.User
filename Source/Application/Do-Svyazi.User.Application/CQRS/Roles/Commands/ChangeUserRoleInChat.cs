using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles.Commands;

public record ChangeRoleForUserByIdCommand(Guid userId, Guid chatId, RoleDto role)
    : IRequest;