using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles;

public static class CreateRoleForChat
{
    public record Command(Role role, Guid chatId) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            Chat? chat = await _context.Chats.FindAsync(request.chatId);
            if (chat is null)
            {
                throw new Do_Svyazi_User_NotFoundException(
                    $"Chat with id = {request.chatId} is not created");
            }

            chat.AddRole(request.role);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}