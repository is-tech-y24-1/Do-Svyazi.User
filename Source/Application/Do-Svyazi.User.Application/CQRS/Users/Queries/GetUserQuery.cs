using Do_Svyazi.User.Dtos.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public record GetUserQuery(Guid userId)
    : IRequest<MessengerUserDto>;