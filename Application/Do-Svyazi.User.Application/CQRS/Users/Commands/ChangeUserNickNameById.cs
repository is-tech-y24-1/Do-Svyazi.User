using Do_Svyazi.User.Application.Abstractions.DbContexts;
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
            var messengerUser = await _context.Users.FindAsync(request.UserId);

            if (messengerUser == null) return Unit.Value;

            messengerUser.NickName = request.NickName;
            _context.Users.Update(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}