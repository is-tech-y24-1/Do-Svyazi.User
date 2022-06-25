using Do_Svyazi.User.Dtos.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Queries;

public record GetUsersRequest
    : IRequest<IReadOnlyCollection<MessengerUserDto>>;