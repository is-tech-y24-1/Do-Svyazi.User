using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Do_Svyazi.User.Application.CQRS.Authenticate.Queries;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Handlers;

public class AuthenticateQueryHandler :
    IQueryHandler<LoginRequest, JwtSecurityToken>,
    IQueryHandler<AuthenticateByJwtRequest, AuthenticateResponse>,
    IQueryHandler<GetUsersRequest, IReadOnlyCollection<MessengerUserDto>>
{
    private const string NameType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

    private readonly UserManager<MessengerUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public AuthenticateQueryHandler(UserManager<MessengerUser> userManager, IConfiguration configuration, IMapper mapper)
    {
        _userManager = userManager;
        _configuration = configuration;
        _mapper = mapper;
    }

    public async Task<JwtSecurityToken> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        LoginModel loginModel = request.model;
        MessengerUser user = await GetUserByUsernameOrEmail(loginModel);

        if (!await _userManager.CheckPasswordAsync(user, loginModel.Password))
            throw new UnauthorizedAccessException("Can't login with this credentials");

        IList<string>? userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, $"{user.Id}"),
            new Claim(JwtRegisteredClaimNames.Email, $"{user.Email}"),
        };

        authClaims
            .AddRange(userRoles
                .Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        return GetToken(authClaims);
    }

    public async Task<AuthenticateResponse> Handle(AuthenticateByJwtRequest request, CancellationToken cancellationToken)
    {
        var token = new JwtSecurityToken(request.jwtToken);
        string userNickName = token.Claims.First(x => x.Type == NameType).Value;

        var identityUser = await _userManager.FindByNameAsync(userNickName);
        return new AuthenticateResponse(identityUser.Id, token.ValidTo);
    }

    public async Task<IReadOnlyCollection<MessengerUserDto>> Handle(GetUsersRequest request, CancellationToken cancellationToken)
        => await _userManager.Users
            .ProjectTo<MessengerUserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

        var time = DateTime.UtcNow.AddHours(int.Parse(_configuration["JWT:Expires"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(int.Parse(_configuration["JWT:Expires"])),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        return token;
    }

    private async Task<MessengerUser> GetUserByUsernameOrEmail(LoginModel loginModel)
    {
        MessengerUser? user = default;

        if (loginModel.UserName is not null)
            user = await _userManager.FindByNameAsync(loginModel.UserName);

        if (user is null && loginModel.Email is not null)
            user = await _userManager.FindByEmailAsync(loginModel.Email);

        if (user is null)
            throw new Do_Svyazi_User_NotFoundException($"User {loginModel.UserName} to login was not found");

        return user;
    }
}