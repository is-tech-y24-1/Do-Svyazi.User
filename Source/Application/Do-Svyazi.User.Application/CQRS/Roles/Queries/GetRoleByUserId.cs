using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles.Queries;

public record GetRoleByUserId(Guid userId, Guid chatId)
    : IRequest<RoleDto>;