using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public class DeleteUser : IRequest
{
    public Guid UserId { get; init; }

    public class Handler : IRequestHandler<DeleteUser>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(DeleteUser request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.UserId) ??
                                          throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.UserId} to delete");

            _context.Users.Remove(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}