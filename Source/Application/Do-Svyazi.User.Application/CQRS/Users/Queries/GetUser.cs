using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public record GetUser(Guid userId)
    : IRequest<MessengerUser>;