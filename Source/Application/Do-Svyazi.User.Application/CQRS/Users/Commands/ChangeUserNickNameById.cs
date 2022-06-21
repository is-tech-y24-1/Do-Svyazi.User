using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public class SetUserNickNameById : IRequest
{
    public Guid UserId { get; init; }
    public string NickName { get; init; }

    public class Handler : IRequestHandler<SetUserNickNameById>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(SetUserNickNameById request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.UserId) ??
                                          throw new Do_Svyazi_User_NotFoundException(
                                              $"User with id {request.UserId} to change nickName was not found");

            if (NickNameExists(request.NickName))
                throw new Do_Svyazi_User_BusinessLogicException($"Nickname = {request.NickName} already exists in messenger");

            messengerUser.ChangeNickName(request.NickName);

            _context.Users.Update(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private bool NickNameExists(string nickName) =>
            _context.Users.Any(user => user.NickName == nickName);
    }
}