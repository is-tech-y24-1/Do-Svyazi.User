using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Queries;

public record GetUsersRequest
    : IRequest<IReadOnlyCollection<MessengerUser>>;