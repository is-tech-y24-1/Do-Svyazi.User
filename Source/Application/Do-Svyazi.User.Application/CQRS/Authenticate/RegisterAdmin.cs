using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Do_Svyazi.User.Application.CQRS.Authenticate;

public class RegisterAdmin : IRequest<Unit>
{
    public RegisterModel Model { get; init; }

    public class Handler : IRequestHandler<RegisterAdmin>
    {
        private readonly UserManager<MessageIdentityUser> _userManager;
        private readonly RoleManager<MessageIdentityRole> _roleManager;

        public Handler(UserManager<MessageIdentityUser> userManager, RoleManager<MessageIdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Unit> Handle(RegisterAdmin request, CancellationToken cancellationToken)
        {
            MessageIdentityUser userExists = await _userManager.FindByNameAsync(request.Model.NickName);
            if (userExists != null)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User already exists");
            }

            MessageIdentityUser user = new ()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Model.NickName,
            };
            IdentityResult? result = await _userManager.CreateAsync(user, request.Model.Password);
            if (!result.Succeeded)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User creation failed! Please check user details and try again.");
            }

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
    }
}