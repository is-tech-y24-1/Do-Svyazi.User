using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles.Queries;

public record GetRoleByUserIdQuery(Guid userId, Guid chatId)
    : IRequest<RoleDto>;