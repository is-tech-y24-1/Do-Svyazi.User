using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class ChangeUserDescriptionById
{
    public record Command(Guid userId, string description) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var messengerUser = await _context.Users.FindAsync(request.userId) ??
                                throw new Do_Svyazi_User_NotFoundException($"User with id {request.userId} not found");

            messengerUser.ChangeDescription(request.description);
            _context.Users.Update(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}