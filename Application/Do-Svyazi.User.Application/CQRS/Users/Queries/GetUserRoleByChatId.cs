using System.IO.Pipes;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public static class GetUserRoleByChatId
{
    public record Query(Guid userId, Guid chatId) : IRequest<Response>;
    public record Response(Role role);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.userId) ??
                                          throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId} to get role in chat with id = {request.chatId}");

            Chat chat = await _context.Chats.FindAsync(request.chatId) ??
                        throw new Do_Svyazi_User_NotFoundException($"Can't find chat with id = {request.chatId} to get role for user with id {request.userId}");

            ChatUser user = chat.GetUser(messengerUser.Id);

            return new Response(user.Role);
        }
    }
}