using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public class GetUser : IRequest<MessengerUser>
{
    public Guid UserId { get; init; }

    public class Handler : IRequestHandler<GetUser, MessengerUser>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<MessengerUser> Handle(GetUser request, CancellationToken cancellationToken)
        {
            MessengerUser user = await _context.Users.FindAsync(request.UserId) ??
                                 throw new Do_Svyazi_User_NotFoundException($"User with id = {request.UserId} doesn't exist");

            return user;
        }
    }
}