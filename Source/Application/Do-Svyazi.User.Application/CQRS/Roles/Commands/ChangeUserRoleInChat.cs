using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Roles.Commands;

public class ChangeRoleForUserById : IRequest
{
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }
    public RoleDto Role { get; init; }

    public class Handler : IRequestHandler<ChangeRoleForUserById, Unit>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(ChangeRoleForUserById request, CancellationToken cancellationToken)
        {
            ChatUser chatUser =
                await _context.ChatUsers
                    .Include(chatUser => chatUser.Role)
                    .FirstOrDefaultAsync(
                        user => user.MessengerUserId == request.UserId && user.ChatId == request.ChatId, cancellationToken: cancellationToken) ??
                throw new Do_Svyazi_User_NotFoundException(
                    $"Chat user with userId = {request.UserId} and chatId = {request.ChatId} to change user role in chat was not found");

            Chat chat = await _context.Chats.FindAsync(request.ChatId) ??
                        throw new Do_Svyazi_User_NotFoundException(
                            $"Can't find chat with id = {request.ChatId} to change user role in chat");

            Role newRole = new ()
            {
                Chat = chat,
                Name = request.Role.Name,
                CanAddUsers = request.Role.CanAddUsers,
                CanDeleteChat = request.Role.CanDeleteChat,
                CanDeleteMessages = request.Role.CanDeleteMessages,
                CanDeleteUsers = request.Role.CanDeleteUsers,
                CanEditMessages = request.Role.CanEditMessages,
                CanPinMessages = request.Role.CanPinMessages,
                CanReadMessages = request.Role.CanReadMessages,
                CanWriteMessages = request.Role.CanWriteMessages,
                CanEditChannelDescription = request.Role.CanEditChannelDescription,
                CanInviteOtherUsers = request.Role.CanInviteOtherUsers,
                CanSeeChannelMembers = request.Role.CanSeeChannelMembers,
            };

            chatUser.ChangeRole(newRole);

            // TODO: debug, if old role doesn't have PK equals to chatUser.MessengerUser
            // maybe it will make sense to remove old role from DB... (not sure)
            _context.ChatUsers.Update(chatUser);
            await _context.Roles.AddAsync(newRole, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}