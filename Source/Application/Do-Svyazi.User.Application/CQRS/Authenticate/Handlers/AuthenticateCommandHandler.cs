using Do_Svyazi.User.Application.CQRS.Authenticate.Commands;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Handlers;

public class AuthenticateCommandHandler :
    ICommandHandler<Register, Unit>,
    ICommandHandler<RegisterAdmin, Unit>
{
    private readonly UserManager<MessageIdentityUser> _userManager;
    private readonly RoleManager<MessageIdentityRole> _roleManager;
    private readonly IDbContext _context;

    public AuthenticateCommandHandler(
        UserManager<MessageIdentityUser> userManager,
        RoleManager<MessageIdentityRole> roleManager,
        IDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<Unit> Handle(Register request, CancellationToken cancellationToken)
    {
        var userModel = request.model;

        if (await _userManager.FindByNameAsync(userModel.NickName) is not null)
            throw new Do_Svyazi_User_BusinessLogicException("User already exists");

        var userId = await GetMessengerUserIdByNickName(userModel.NickName, cancellationToken);

        MessageIdentityUser user = new ()
        {
            SecurityStamp = $"{userId}",
            UserName = userModel.NickName,
            Email = userModel.Email,
        };

        if (!(await _userManager.CreateAsync(user, userModel.Password)).Succeeded)
            throw new Do_Svyazi_User_BusinessLogicException("User creation failed! Please check user details and try again.");

        return Unit.Value;
    }

    public async Task<Unit> Handle(RegisterAdmin request, CancellationToken cancellationToken)
    {
        var userModel = request.model;

        if (await _userManager.FindByNameAsync(userModel.NickName) is not null)
            throw new Do_Svyazi_User_BusinessLogicException("User already exists");

        var userId = await GetMessengerUserIdByNickName(userModel.NickName, cancellationToken);

        MessageIdentityUser user = new ()
        {
            SecurityStamp = $"{userId}",
            UserName = userModel.NickName,
        };

        if (!(await _userManager.CreateAsync(user, userModel.Password)).Succeeded)
            throw new Do_Svyazi_User_BusinessLogicException("User creation failed! Please check user details and try again.");

        if (await _roleManager.RoleExistsAsync(MessageIdentityRole.Admin))
        {
            await _userManager.AddToRoleAsync(user, MessageIdentityRole.Admin);
            await _roleManager.CreateAsync(new MessageIdentityRole(MessageIdentityRole.User));
        }

        if (await _roleManager.RoleExistsAsync(MessageIdentityRole.User))
        {
            await _userManager.AddToRoleAsync(user, MessageIdentityRole.User);
            await _roleManager.CreateAsync(new MessageIdentityRole(MessageIdentityRole.Admin));
        }

        return Unit.Value;
    }

    private async Task<Guid> GetMessengerUserIdByNickName(string nickName, CancellationToken cancellationToken) =>
        (await _context.Users.SingleAsync(user => user.NickName == nickName, cancellationToken: cancellationToken)).Id;
}