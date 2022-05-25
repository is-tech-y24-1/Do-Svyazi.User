using Do_Svyazi.User.Application.Abstractions.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var messengerUser = await _context.Users
                .SingleOrDefaultAsync(
                    user => user.Id == request.UserId, cancellationToken);

            if (messengerUser == null) return Unit.Value;

            messengerUser.NickName = request.NickName;
            _context.Users.Update(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}