using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public class GetUserRoleByChatId : IRequest<Role>
{
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }

    public class Handler : IRequestHandler<GetUserRoleByChatId, Role>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<Role> Handle(GetUserRoleByChatId request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.UserId) ??
                                          throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.UserId} to get role in chat with id = {request.ChatId}");

            Chat chat = await _context.Chats
                            .Include(chat => chat.Users)
                                .ThenInclude(user => user.Role)
                            .FirstOrDefaultAsync(chat => chat.Id == request.ChatId, cancellationToken)
                        ?? throw new Do_Svyazi_User_NotFoundException($"Can't find chat with id = {request.ChatId} to get role for user with id {request.UserId}");

            ChatUser user = chat.GetUser(messengerUser.Id);

            return user.Role;
        }
    }
}