using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class SetUserNickNameById
{
    public record Command(Guid userId, string nickName) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.userId) ??
                                          throw new Do_Svyazi_User_NotFoundException(
                                              $"User with id {request.userId} to change nickName was not found");

            if (NickNameExists(request.nickName))
                throw new Do_Svyazi_User_BusinessLogicException($"Nickname = {request.nickName} already exists in messenger");

            messengerUser.ChangeNickName(request.nickName);

            _context.Users.Update(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private bool NickNameExists(string nickName) =>
            _context.Users.Any(user => user.NickName == nickName);
    }
}