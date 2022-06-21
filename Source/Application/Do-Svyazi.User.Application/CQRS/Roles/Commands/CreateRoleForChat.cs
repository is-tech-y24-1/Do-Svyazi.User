using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles.Commands;

public class CreateRoleForChat : IRequest
{
    public RoleDto Role { get; init; }
    public Guid ChatId { get; init; }

    public class Handler : IRequestHandler<CreateRoleForChat, Unit>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<Unit> Handle(CreateRoleForChat request, CancellationToken cancellationToken)
        {
            Chat chat = await _context.Chats.FindAsync(request.ChatId)
                        ?? throw new Do_Svyazi_User_NotFoundException($"Chat with id = {request.ChatId} to create role was not found");

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

            chat.AddRole(newRole);

            _context.Chats.Update(chat);
            await _context.Roles.AddAsync(newRole, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}