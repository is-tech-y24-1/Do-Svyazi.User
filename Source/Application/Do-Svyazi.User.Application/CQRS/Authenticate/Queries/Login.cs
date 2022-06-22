using System.IdentityModel.Tokens.Jwt;
using Do_Svyazi.User.Domain.Authenticate;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Queries;

public record Login(LoginModel model)
    : IRequest<JwtSecurityToken>;