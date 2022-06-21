using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Do_Svyazi.User.Application.CQRS.Authenticate;

public class Register : IRequest
{
    public RegisterModel Model { get; init; }

    public class Handler : IRequestHandler<Register>
    {
        private readonly UserManager<MessageIdentityUser> _userManager;
        public Handler(UserManager<MessageIdentityUser> userManager) => _userManager = userManager;

        public async Task<Unit> Handle(Register request, CancellationToken cancellationToken)
        {
            MessageIdentityUser? userExists = await _userManager.FindByNameAsync(request.Model.NickName);

            if (userExists != null)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User already exists");
            }

            MessageIdentityUser user = new ()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Model.NickName,
                Email = request.Model.Email,
            };

            IdentityResult? result = await _userManager.CreateAsync(user, request.Model.Password);
            if (!result.Succeeded)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User creation failed! Please check user details and try again.");
            }

            return Unit.Value;
        }
    }
}