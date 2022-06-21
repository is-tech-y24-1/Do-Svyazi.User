using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public class GetAllChatsIdsByUserId : IRequest<IReadOnlyList<Guid>>
{
    public Guid UserId { get; init; }

    public class Handler : IRequestHandler<GetAllChatsIdsByUserId, IReadOnlyList<Guid>>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<IReadOnlyList<Guid>> Handle(GetAllChatsIdsByUserId request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.UserId) ??
                                          throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.UserId} to get user chat ids");

            var chatIds = messengerUser.Chats.Select(chat => chat.Id).ToList();

            return chatIds;
        }
    }
}