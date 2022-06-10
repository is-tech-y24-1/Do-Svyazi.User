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

public static class ChangeRoleForUserById
{
    public record Command(Guid userId, Guid chatId,  RoleDto role) : IRequest;

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
            ChatUser? chatUser = await _context.ChatUsers
                                     .Include(chatUser => chatUser.Role)
                                     .FirstOrDefaultAsync(
                                         user => user.User.Id == request.userId
                                                 && user.ChatId == request.chatId,
                                         cancellationToken: cancellationToken) ??
                                 throw new Do_Svyazi_User_NotFoundException(
                                     $"Chat user with userId = {request.userId} and chatId = {request.chatId} not found");

            Chat chat = await _context.Chats.FindAsync(request.chatId) ??
                        throw new Do_Svyazi_User_NotFoundException($"Can't find chat with id = {request.chatId}");

            Role? newRole = new Role()
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
                CanEditChannelDescription = request.role.CanEditMessages,
                CanInviteOtherUsers = request.role.CanInviteOtherUsers,
                CanSeeChannelMembers = request.role.CanSeeChannelMembers,
            };

            chatUser.ChangeRole(newRole);
            _context.ChatUsers.Update(chatUser);
            _context.Roles.Add(newRole);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}