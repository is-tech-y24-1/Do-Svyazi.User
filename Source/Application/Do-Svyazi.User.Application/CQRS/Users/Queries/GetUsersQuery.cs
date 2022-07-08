using Do_Svyazi.User.Dtos.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public record GetUsersQuery
    : IRequest<IReadOnlyCollection<MessengerUserDto>>;