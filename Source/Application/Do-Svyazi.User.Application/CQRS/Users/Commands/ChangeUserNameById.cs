using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public class ChangeUserNameById : IRequest
{
    public Guid UserId { get; init; }
    public string Name { get; init; }

    public class Handler : IRequestHandler<ChangeUserNameById>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(ChangeUserNameById request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.UserId) ??
                                          throw new Do_Svyazi_User_NotFoundException($"User with id {request.UserId} to change name was not found");

            messengerUser.ChangeName(request.Name);

            _context.Users.Update(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}