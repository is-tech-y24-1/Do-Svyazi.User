using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public class AddPersonalChat
{
    public record Command(Guid firstUserId, Guid secondUserId, string name, string description) : IRequest<Guid>;

    public class Handler : IRequestHandler<Command, Guid>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser firstUser = await _context.Users.SingleOrDefaultAsync(user => user.Id == request.firstUserId, cancellationToken) ??
                                           throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.firstUserId}");

            MessengerUser secondUser = await _context.Users
                                           .SingleOrDefaultAsync(user => user.Id == request.secondUserId, cancellationToken) ??
                                           throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.secondUserId}");

            Chat chat = new PersonalChat(firstUser, secondUser, request.name, request.description);

            await _context.Chats.AddAsync(chat, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return chat.Id;
        }
    }
}