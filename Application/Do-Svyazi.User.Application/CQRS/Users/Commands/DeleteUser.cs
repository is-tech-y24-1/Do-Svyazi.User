using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class DeleteUser
{
    public record Command(Guid userId) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser? foundedUser = await _context.Users.FindAsync(request.userId);
            if (foundedUser is null)
            {
                throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId}");
            }

            _context.Users.Remove(foundedUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}