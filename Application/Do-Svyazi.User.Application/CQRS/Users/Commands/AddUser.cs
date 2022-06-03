using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class AddUser
{
    public record Command(string name, string nickName, string description) : IRequest<Guid>;

    public class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            if (IsUserExist(request.nickName))
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    $"User with nickname = {request.nickName} is already created");
            }

            var messengerUser = new MessengerUser(request.name, request.nickName, request.description);
            await _context.Users.AddAsync(messengerUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return messengerUser.Id;
        }

        private bool IsUserExist(string nickName)
        {
            MessengerUser? foundUser = _context.Users.FirstOrDefault(user => user.NickName == nickName);
            return foundUser is not null;
        }
    }
}