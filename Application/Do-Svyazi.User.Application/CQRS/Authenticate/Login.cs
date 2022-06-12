using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Authenticate;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public static class Login
{
    public record Command(LoginModel model) : IRequest<JwtSecurityToken>;

    public class Handler : IRequestHandler<Command, JwtSecurityToken>
    {
        private readonly UserManager<MessageIdentityUser> _userManager;
        private readonly RoleManager<MessageIdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public Handler(IDbContext context, UserManager<MessageIdentityUser> userManager, RoleManager<MessageIdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<JwtSecurityToken> Handle(Command request, CancellationToken cancellationToken)
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

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return token;
        }
    }
}