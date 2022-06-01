using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class GetAllChatsIdByUserId
{
    public record Command(Guid userId) : IRequest<IReadOnlyList<Guid>>;

    public class Handler : IRequestHandler<Command, IReadOnlyList<Guid>>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<IReadOnlyList<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser? messengerUser = await _context.Users.FindAsync(request.userId) ??
                throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId}");

            return messengerUser.Chats.Select(chat => chat.Id).ToList();
        }
    }
}