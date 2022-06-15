using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public static class Register
{
    public record Command(RegisterModel model) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly UserManager<MessageIdentityUser> _userManager;
        public Handler(UserManager<MessageIdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            MessageIdentityUser? userExists = await _userManager.FindByNameAsync(request.model.NickName);

            if (userExists != null)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User already exists");
            }

            MessageIdentityUser user = new ()
            {
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.model.NickName,
                Email = request.model.Email,
            };

            IdentityResult? result = await _userManager.CreateAsync(user, request.model.Password);
            if (!result.Succeeded)
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    "User creation failed! Please check user details and try again.");
            }

            return Unit.Value;
        }
    }
}