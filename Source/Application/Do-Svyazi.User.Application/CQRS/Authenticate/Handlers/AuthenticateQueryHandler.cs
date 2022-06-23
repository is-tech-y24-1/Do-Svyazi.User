using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Do_Svyazi.User.Application.CQRS.Authenticate.Queries;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Domain.Authenticate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Handlers;

public class AuthenticateQueryHandler :
    IQueryHandler<Login, JwtSecurityToken>,
    IQueryHandler<AuthenticateByJwt, Guid>
{
    private readonly UserManager<MessageIdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticateQueryHandler(UserManager<MessageIdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<JwtSecurityToken> Handle(Login request, CancellationToken cancellationToken)
    {
        var token = new JwtSecurityToken();
        MessageIdentityUser user = await _userManager.FindByNameAsync(request.model.NickName);
        if (user != null && await _userManager.CheckPasswordAsync(user, request.model.Password))
        {
            IList<string>? userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            authClaims
                .AddRange(userRoles
                    .Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            token = GetToken(authClaims);
        }

        return token;
    }

    public async Task<Guid> Handle(AuthenticateByJwt request, CancellationToken cancellationToken)
    {
        var token = new JwtSecurityToken(request.jwtToken);
        string userNickName = token.Claims.First(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

        var identityUser = await _userManager.FindByNameAsync(userNickName);

        return identityUser.Id;
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddHours(int.Parse(_configuration["JWT:Expires"])),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return token;
    }
}