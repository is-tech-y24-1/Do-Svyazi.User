using Do_Svyazi.User.Dtos.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public record GetUsers
    : IRequest<IReadOnlyCollection<MessengerUserDto>>;