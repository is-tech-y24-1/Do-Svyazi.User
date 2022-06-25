using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.CQRS.Roles.Commands;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Roles.Handlers;

public class RolesCommandHandler :
    ICommandHandler<ChangeRoleForUserById, Unit>,
    IRequestHandler<CreateRoleForChat, Unit>
{
    private readonly IDbContext _context;
    public RolesCommandHandler(IDbContext context) => _context = context;

    public async Task<Unit> Handle(ChangeRoleForUserById request, CancellationToken cancellationToken)
    {
        Chat chat = await _context.Chats.FindAsync(request.chatId) ??
                    throw new Do_Svyazi_User_NotFoundException(
                        $"Can't find chat with id = {request.chatId} to change user role in chat");

        Role role = await _context.Roles
            .Include(role => role.Chat)
            .SingleAsync(
                role =>
                (role.Name == request.role.Name)
                && (role.Chat.Id == request.chatId),
                cancellationToken: cancellationToken);

        role.Name = request.role.Name;
        role.CanAddUsers = request.role.CanAddUsers;
        role.CanDeleteChat = request.role.CanDeleteChat;
        role.CanDeleteMessages = request.role.CanDeleteMessages;
        role.CanDeleteUsers = request.role.CanDeleteUsers;
        role.CanEditMessages = request.role.CanEditMessages;
        role.CanPinMessages = request.role.CanPinMessages;
        role.CanReadMessages = request.role.CanReadMessages;
        role.CanWriteMessages = request.role.CanWriteMessages;
        role.CanEditChannelDescription = request.role.CanEditChannelDescription;
        role.CanInviteOtherUsers = request.role.CanInviteOtherUsers;
        role.CanSeeChannelMembers = request.role.CanSeeChannelMembers;
        _context.Roles.Update(role);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    public async Task<Unit> Handle(CreateRoleForChat request, CancellationToken cancellationToken)
    {
        Chat chat = await _context.Chats.FindAsync(request.chatId)
                    ?? throw new Do_Svyazi_User_NotFoundException(
                        $"Chat with id = {request.chatId} to create role was not found");

        var newRole = new Role
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

        chat.AddRole(newRole);

        _context.Chats.Update(chat);
        await _context.Roles.AddAsync(newRole, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}