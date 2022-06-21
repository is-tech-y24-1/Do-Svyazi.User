using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Do_Svyazi.User.Domain.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Do_Svyazi.User.Application.CQRS.Authenticate;

public class Login : IRequest<JwtSecurityToken>
{
    public LoginModel Model { get; init; }

    public class Handler : IRequestHandler<Login, JwtSecurityToken>
    {
        private readonly UserManager<MessageIdentityUser> _userManager;
        private readonly IConfiguration _configuration;

        public Handler(UserManager<MessageIdentityUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> Handle(Login request, CancellationToken cancellationToken)
        {
            var token = new JwtSecurityToken();
            MessageIdentityUser user = await _userManager.FindByNameAsync(request.Model.NickName);
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Model.Password))
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
}