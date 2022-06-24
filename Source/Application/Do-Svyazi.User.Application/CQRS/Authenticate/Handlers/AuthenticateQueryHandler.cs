using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Do_Svyazi.User.Application.CQRS.Authenticate.Queries;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Handlers;

public class AuthenticateQueryHandler :
    IQueryHandler<LoginRequest, JwtSecurityToken>,
    IQueryHandler<AuthenticateByJwtRequest, Guid>,
    IQueryHandler<GetUsersRequest, IReadOnlyCollection<MessageIdentityUser>>
{
    private const string NameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

    private readonly UserManager<MessageIdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthenticateQueryHandler(UserManager<MessageIdentityUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<JwtSecurityToken> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        var token = new JwtSecurityToken();
        LoginModel loginModel = request.model;
        MessageIdentityUser user = await GetUserByUsernameOrEmail(loginModel);

        if (!await _userManager.CheckPasswordAsync(user, loginModel.Password)) return token;

        IList<string>? userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, $"{user.Id}"),
        };

        authClaims
            .AddRange(userRoles
                .Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        return GetToken(authClaims);
    }

    public async Task<Guid> Handle(AuthenticateByJwtRequest request, CancellationToken cancellationToken)
    {
        var token = new JwtSecurityToken(request.jwtToken);
        string userNickName = token.Claims.First(x => x.Type == NameType).Value;

        var identityUser = await _userManager.FindByNameAsync(userNickName);
        return identityUser.Id;
    }

    public async Task<IReadOnlyCollection<MessageIdentityUser>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        => await _userManager.Users.ToListAsync(cancellationToken: cancellationToken);

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

    private async Task<MessageIdentityUser> GetUserByUsernameOrEmail(LoginModel loginModel)
    {
        if (loginModel.NickName is not null)
            return await _userManager.FindByNameAsync(loginModel.NickName);

        if (loginModel.Email is not null)
            return await _userManager.FindByEmailAsync(loginModel.Email);

        throw new Do_Svyazi_User_NotFoundException("User to login was not found");
    }
}