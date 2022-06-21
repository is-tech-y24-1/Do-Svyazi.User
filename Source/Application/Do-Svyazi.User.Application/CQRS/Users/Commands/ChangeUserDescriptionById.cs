using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public class ChangeUserDescriptionById : IRequest
{
    public Guid UserId { get; init; }
    public string Description { get; init; }

    public class Handler : IRequestHandler<ChangeUserDescriptionById>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(ChangeUserDescriptionById request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.UserId) ??
                                          throw new Do_Svyazi_User_NotFoundException(
                                              $"User with id {request.UserId} to change description was not found");

            messengerUser.ChangeDescription(request.Description);

            _context.Users.Update(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}