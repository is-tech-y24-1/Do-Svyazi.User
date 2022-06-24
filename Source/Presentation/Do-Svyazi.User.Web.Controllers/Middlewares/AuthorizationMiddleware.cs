using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Do_Svyazi.User.Domain.Authenticate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Do_Svyazi.User.Web.Controllers.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly UserManager<MessageIdentityUser> _userManager;

    public AuthorizationMiddleware(
        RequestDelegate next, IConfiguration configuration, UserManager<MessageIdentityUser> userManager)
    {
        _next = next;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
            await AttachUserToContext(context, token);

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero,
                }, out SecurityToken validatedToken);

            var jwtToken = validatedToken as JwtSecurityToken;
            string? userId = jwtToken?.Claims.First(x => x.Type == "jti").Value;

            var user = await _userManager.FindByIdAsync(userId);
            context.Items["User"] = user;
        }
        catch
        {
            throw new UnauthorizedAccessException();
        }
    }
}