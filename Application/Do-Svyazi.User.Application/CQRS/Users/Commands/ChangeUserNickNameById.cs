using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class SetUserNickNameById
{
    public record Command(Guid UserId, string NickName) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var messengerUser = await _context.Users.FindAsync(request.UserId) ??
                                throw new Do_Svyazi_User_NotFoundException($"User with id {request.UserId} not found");

            messengerUser.NickName = request.NickName;
            _context.Users.Update(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}