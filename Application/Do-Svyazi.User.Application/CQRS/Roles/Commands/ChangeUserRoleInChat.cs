using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Roles;

public static class ChangeUserRoleInChat
{
    public record Command(Guid userId, Guid chatId, Guid chatUserId,  RoleDto newRole) : IRequest<ChatAndUserId>;

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
            ChatUser? chatUser = await _context.ChatUsers
                                     .Include(chatUser => chatUser.Role)
                                     .FirstOrDefaultAsync(
                                         user => user.User.Id == request.userId
                                                 && user.ChatId == request.chatId,
                                         cancellationToken: cancellationToken) ??
                                 throw new Do_Svyazi_User_NotFoundException(
                                     $"Chat user with userId = {request.userId} and chatId = {request.chatId} not found");

            var role = _mapper.Map<Role>(request.newRole);
            chatUser.ChangeRole(role);
            _context.Roles.Add(role);
            _context.ChatUsers.Update(chatUser);
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