using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles;

public static class ChangeUserRoleInChat
{
    public record Command(Guid userId, Guid chatId, RoleDto newRole) : IRequest<ChatAndUserId>;

    public class Handler : IRequestHandler<Command, ChatAndUserId>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ChatAndUserId> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser? messengerUser = await _context.Users.FindAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"Can't find user with id = {request.userId}");

            Chat chat = await _context.Chats.FindAsync(request.chatId) ??
                        throw new Do_Svyazi_User_NotFoundException($"Can't find chat with id = {request.chatId}");
            if (request.newRole is null)
            {
                throw new Do_Svyazi_User_BusinessLogicException("Roles to set is null");
            }

            chat.ChangeUserRole(messengerUser, _mapper.Map<Role>(request.newRole));
            _context.Users.Update(messengerUser);
            _context.Roles.Add(_mapper.Map<Role>(request.newRole));
            await _context.SaveChangesAsync(cancellationToken);

            var chatAndUserId = new ChatAndUserId()
            {
                ChatId = request.chatId,
                UserId = request.userId,
            };

            return chatAndUserId;
        }
    }
}