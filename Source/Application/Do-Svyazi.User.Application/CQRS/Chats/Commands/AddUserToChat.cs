using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public class AddUserToChat : IRequest<Unit>
{
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }

    public class Handler : IRequestHandler<AddUserToChat, Unit>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(AddUserToChat request, CancellationToken cancellationToken)
        {
            Chat chat = await _context.Chats
                            .Include(chat => chat.Users)
                                .ThenInclude(user => user.User)
                            .Include(chat => chat.Users)
                                .ThenInclude(user => user.Role)
                            .SingleOrDefaultAsync(chat => chat.Id == request.ChatId, cancellationToken) ??
                        throw new Do_Svyazi_User_NotFoundException(
                            $"Chat with id = {request.ChatId} to add user {request.UserId} was not found");

            MessengerUser messengerUser = await _context.Users
                                              .SingleOrDefaultAsync(user => user.Id == request.UserId, cancellationToken) ??
                                          throw new Do_Svyazi_User_NotFoundException(
                                              $"User with id = {request.UserId} to be added into chat with id = {request.ChatId} not found");

            ChatUser newChatUser = chat.AddUser(messengerUser);

            await _context.ChatUsers.AddAsync(newChatUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}