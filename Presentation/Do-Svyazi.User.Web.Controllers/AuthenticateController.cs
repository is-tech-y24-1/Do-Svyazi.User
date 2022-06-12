using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Do_Svyazi.User.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<MessageIdentityUser> _userManager;
        private readonly RoleManager<MessageIdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(
            UserManager<MessageIdentityUser> userManager,
            RoleManager<MessageIdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            MessageIdentityUser user = await _userManager.FindByNameAsync(model.NickName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
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

                JwtSecurityToken token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                });
            }

            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            MessageIdentityUser? userExists = await _userManager.FindByNameAsync(model.NickName);

            if (userExists != null)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User already exists");
            }

            MessageIdentityUser user = new ()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.NickName,
            };

            IdentityResult? result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User creation failed! Please check user details and try again.");
            }

            return Ok();
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            MessageIdentityUser userExists = await _userManager.FindByNameAsync(model.NickName);
            if (userExists != null)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User already exists");
            }

            MessageIdentityUser user = new ()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.NickName,
            };
            IdentityResult? result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User creation failed! Please check user details and try again.");
            }

            if (!await _roleManager.RoleExistsAsync(MessageIdentityRole.Admin))
                await _roleManager.CreateAsync(new MessageIdentityRole(MessageIdentityRole.Admin));
            if (!await _roleManager.RoleExistsAsync(MessageIdentityRole.User))
                await _roleManager.CreateAsync(new MessageIdentityRole(MessageIdentityRole.User));

            if (await _roleManager.RoleExistsAsync(MessageIdentityRole.Admin))
            {
                await _userManager.AddToRoleAsync(user, MessageIdentityRole.Admin);
            }

            if (await _roleManager.RoleExistsAsync(MessageIdentityRole.Admin))
            {
                await _userManager.AddToRoleAsync(user, MessageIdentityRole.User);
            }

            return Ok();
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