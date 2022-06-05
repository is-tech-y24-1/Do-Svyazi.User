using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles;

public static class ChangeChatRole
{
    public record Command(Guid userId, Guid chatId, RoleDto newRole) : IRequest;

    public class Handler : IRequestHandler<Command>
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
            MessengerUser? chatUser = await _context.Users.FindAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"Can't find user with id = {request.userId}");

            Chat chat = await _context.Chats.FindAsync(request.chatId) ??
                        throw new Do_Svyazi_User_NotFoundException($"Can't find chat with id = {request.chatId}");
            if (request.newRole is null)
            {
                throw new Do_Svyazi_User_BusinessLogicException("Role to set is null");
            }

            chat.ChangeUserRole(chatUser, _mapper.Map<Role>(request.newRole));
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}