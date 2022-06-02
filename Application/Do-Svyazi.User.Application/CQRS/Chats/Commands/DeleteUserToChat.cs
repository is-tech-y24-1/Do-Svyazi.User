using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public class DeleteUserToChat
{
    public record Command(Guid userId, Guid chatId) : IRequest;

    public class Handler : IRequestHandler<Command>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            Chat chat = await _context.Chats
                               .SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                           throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.chatId}");

            MessengerUser messengerUser = await _context.Users
                                              .SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken) ??
                                           throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId}");

            chat.RemoveUser(messengerUser);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}