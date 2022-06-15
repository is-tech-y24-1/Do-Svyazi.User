using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public static class GetUser
{
    public record Query(Guid userId) : IRequest<Response>;
    public record Response(MessengerUser messengerUser);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            MessengerUser user = await _context.Users.FindAsync(request.userId) ??
                                 throw new Do_Svyazi_User_NotFoundException($"User with id = {request.userId} doesn't exist");

            return new Response(user);
        }
    }
}