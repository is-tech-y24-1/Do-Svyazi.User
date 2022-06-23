using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Queries;

public record AuthenticateByJwt(string jwtToken)
    : IRequest<Guid>;