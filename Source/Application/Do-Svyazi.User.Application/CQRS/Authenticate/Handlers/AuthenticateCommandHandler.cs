using Do_Svyazi.User.Application.CQRS.Authenticate.Commands;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Authenticate.Handlers;

public class AuthenticateCommandHandler :
    ICommandHandler<RegisterCommand, Guid>,
    ICommandHandler<RegisterAdminCommand, Unit>
{
    private readonly UserManager<MessengerUser> _userManager;
    private readonly RoleManager<MessageIdentityRole> _roleManager;

    public AuthenticateCommandHandler(
        UserManager<MessengerUser> userManager,
        RoleManager<MessageIdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<Guid> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        RegisterModel registerModel = request.model;

        if (await _userManager.FindByNameAsync(registerModel.UserName) is not null)
            throw new Do_Svyazi_User_BusinessLogicException("User already exists");

        MessengerUser user = CreateIdentityUser(registerModel);

        if (!(await _userManager.CreateAsync(user, registerModel.Password)).Succeeded)
            throw new Do_Svyazi_User_BusinessLogicException("User creation failed! Please check user details and try again.");

        return user.Id;
    }

    public async Task<Unit> Handle(RegisterAdminCommand request, CancellationToken cancellationToken)
    {
        RegisterModel registerModel = request.model;

        if (await _userManager.FindByNameAsync(registerModel.UserName) is not null)
            throw new Do_Svyazi_User_BusinessLogicException($"User with userName {registerModel.UserName} exists");

        MessengerUser user = CreateIdentityUser(registerModel);

        if (!(await _userManager.CreateAsync(user, registerModel.Password)).Succeeded)
            throw new Do_Svyazi_User_BusinessLogicException("User creation failed! Please check user details and try again.");

        if (!await _roleManager.RoleExistsAsync(MessageIdentityRole.Admin))
            await _roleManager.CreateAsync(new MessageIdentityRole(MessageIdentityRole.Admin));

        if (!await _roleManager.RoleExistsAsync(MessageIdentityRole.User))
            await _roleManager.CreateAsync(new MessageIdentityRole(MessageIdentityRole.User));

        if (await _roleManager.RoleExistsAsync(MessageIdentityRole.Admin))
            await _userManager.AddToRoleAsync(user, MessageIdentityRole.Admin);

        if (await _roleManager.RoleExistsAsync(MessageIdentityRole.Admin))
            await _userManager.AddToRoleAsync(user, MessageIdentityRole.User);

        return Unit.Value;
    }

    private MessengerUser CreateIdentityUser(RegisterModel userModel) =>
        new (userModel.Name, userModel.UserName, userModel.Email, userModel.PhoneNumber);
}