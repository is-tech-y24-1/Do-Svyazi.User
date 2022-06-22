using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles.Commands;

public record CreateRoleForChat(RoleDto role, Guid chatId)
    : IRequest;