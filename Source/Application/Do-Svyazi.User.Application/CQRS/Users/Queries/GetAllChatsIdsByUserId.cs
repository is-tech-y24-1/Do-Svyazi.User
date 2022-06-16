using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public static class GetAllChatsIdsByUserId
{
    public record Query(Guid userId) : IRequest<Response>;
    public record Response(IReadOnlyList<Guid> chatIds);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.userId) ??
                                          throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId} to get user chat ids");

            var chatIds = messengerUser.Chats.Select(chat => chat.Id).ToList();

            return new Response(chatIds);
        }
    }
}