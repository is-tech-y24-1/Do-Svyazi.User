using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class GetUserRoleByChatId
{
    public record Command(Guid userId, Guid chatId) : IRequest<Role>;

    public class Handler : IRequestHandler<Command, Role>
    {
        private readonly IUsersAndChatDbContext _context;

        public Handler(IUsersAndChatDbContext context) => _context = context;

        public async Task<Role> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser? foundedUser = await _context.Users.FindAsync(request.userId);
            if (foundedUser is null)
                throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId}");
            Chat? chat = await _context.Chats.FindAsync(request.chatId);

            if (chat is null)
                throw new Do_Svyazi_User_NotFoundException($"Can't find chat with id = {request.chatId}");

            return chat.Users
                .Where(user => user.UserId == foundedUser.Id)
                .Select(user => user.Role)
                .First();
        }
    }
}