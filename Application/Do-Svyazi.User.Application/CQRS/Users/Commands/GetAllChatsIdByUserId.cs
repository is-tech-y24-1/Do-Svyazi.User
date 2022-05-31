using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class GetAllChatsIdByUserId
{
    public record Command(Guid userId) : IRequest<List<Guid>>;

    public class Handler : IRequestHandler<Command, List<Guid>>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<List<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser? foundedUser = await _context.Users.FindAsync(request.userId);
            if (foundedUser is null)
                throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId}");

            return foundedUser.Chats.Select(chat => chat.Id).ToList();
        }
    }
}