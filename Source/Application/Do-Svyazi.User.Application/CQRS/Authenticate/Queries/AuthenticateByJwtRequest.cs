using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Queries;

public record AuthenticateByJwtRequest(string jwtToken)
    : IRequest<Guid>;