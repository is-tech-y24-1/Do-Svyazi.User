using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class AddChatToUser
{
    public record Command(Guid userId, Guid chatId) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.userId) ??
                                          throw new Do_Svyazi_User_NotFoundException($"User with id {request.userId} not found");

            Chat chat = await _context.Chats.FindAsync(request.chatId) ??
                        throw new Do_Svyazi_User_NotFoundException($"Chat with id {request.chatId} not found");

            messengerUser.Chats.Add(chat);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}