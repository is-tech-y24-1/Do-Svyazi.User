using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles;

public static class CreateRoleForChat
{
    public record Command(RoleDto role, Guid chatId) : IRequest<Unit>;

    public class Handler : IRequestHandler<Command, Unit>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            Chat? chat = await _context.Chats.FindAsync(request.chatId);
            if (chat is null)
            {
                throw new Do_Svyazi_User_NotFoundException(
                    $"Chat with id = {request.chatId} is not created");
            }

            Role? newRole = _mapper.Map<Role>(request.role);
            chat.AddRole(newRole);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}