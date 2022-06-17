using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Roles.Commands;

public static class ChangeRoleForUserById
{
    public record Command(Guid userId, Guid chatId, RoleDto role) : IRequest;

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
            ChatUser chatUser =
                await _context.ChatUsers
                    .Include(chatUser => chatUser.Role)
                    .FirstOrDefaultAsync(
                        user => user.MessengerUserId == request.userId && user.ChatId == request.chatId, cancellationToken: cancellationToken) ??
                throw new Do_Svyazi_User_NotFoundException(
                    $"Chat user with userId = {request.userId} and chatId = {request.chatId} to change user role in chat was not found");

            Chat chat = await _context.Chats.FindAsync(request.chatId) ??
                        throw new Do_Svyazi_User_NotFoundException(
                            $"Can't find chat with id = {request.chatId} to change user role in chat");

            Role newRole = new ()
            {
                Chat = chat,
                Name = request.role.Name,
                CanAddUsers = request.role.CanAddUsers,
                CanDeleteChat = request.role.CanDeleteChat,
                CanDeleteMessages = request.role.CanDeleteMessages,
                CanDeleteUsers = request.role.CanDeleteUsers,
                CanEditMessages = request.role.CanEditMessages,
                CanPinMessages = request.role.CanPinMessages,
                CanReadMessages = request.role.CanReadMessages,
                CanWriteMessages = request.role.CanWriteMessages,
                CanEditChannelDescription = request.role.CanEditChannelDescription,
                CanInviteOtherUsers = request.role.CanInviteOtherUsers,
                CanSeeChannelMembers = request.role.CanSeeChannelMembers,
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