using Do_Svyazi.User.Application.Abstractions.DbContexts;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class AddUser
{
    public record Command(string Name, string NickName, string Description) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var user = new MessengerUser
            {
                Name = request.Name,
                NickName = request.NickName,
                Description = request.Description,
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}