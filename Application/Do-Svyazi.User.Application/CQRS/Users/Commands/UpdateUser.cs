using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class UpdateUser
{
    public record Command(Guid userId, MessengerUser user) : IRequest<Guid>, IRequest<MessengerUser>;

    public class Handler : IRequestHandler<Command, MessengerUser>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<MessengerUser> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request.user is null)
                throw new ArgumentNullException(nameof(request.user), $"Invalid user data in request");

            MessengerUser? foundedUser = await _context.Users.FindAsync(request.userId);

            if (foundedUser is null)
                throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId}");

            foundedUser.Chats = request.user.Chats;
            foundedUser.Description = request.user.Description;
            foundedUser.Name = request.user.Name;
            foundedUser.NickName = request.user.NickName;

            await _context.SaveChangesAsync(cancellationToken);
            return foundedUser;
        }
    }
}