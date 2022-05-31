using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class AddUser
{
    public record Command(string name, string nickName, string description) : IRequest<Guid>;

    public class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            var messengerUser = new MessengerUser(request.name, request.nickName, request.description);
            await _context.Users.AddAsync(messengerUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return messengerUser.Id;
        }
    }
}