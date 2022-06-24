using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public record GetUserQuery(Guid userId)
    : IRequest<MessengerUser>;