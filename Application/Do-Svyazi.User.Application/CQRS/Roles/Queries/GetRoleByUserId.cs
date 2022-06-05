using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles;

public static class GetRoleByUserId
{
    public record Command(Guid userId) : IRequest<Role>;

    public class Handler : IRequestHandler<Command, Role>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Role> Handle(Command request, CancellationToken cancellationToken)
        {
            ChatUser? chatUser = await _context.ChatUsers.FindAsync(request.userId) ??
                                           throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId}");

            return chatUser.Role;
        }
    }
}