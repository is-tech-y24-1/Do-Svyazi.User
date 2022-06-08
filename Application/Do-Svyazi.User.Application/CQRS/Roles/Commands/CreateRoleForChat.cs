using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles.Commands;

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
            try
            {
                Chat? chat = await _context.Chats.FindAsync(request.chatId);
                if (chat is null)
                {
                    throw new Do_Svyazi_User_NotFoundException(
                        $"Chat with id = {request.chatId} is not created");
                }

                if (IsRoleNameExist(chat.Roles, request))
                {
                    throw new Do_Svyazi_User_BusinessLogicException(
                        $"Role with name = {request.role.Name} is already exist");
                }

                Role? newRole = new Role()
                {
                    Name = request.role.Name,
                    Chat = chat,
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

                chat.AddRole(newRole);
                _context.Chats.Update(chat);
                await _context.Role.AddAsync(newRole, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
            catch (Exception e)
            {
                throw new Do_Svyazi_User_InnerLogicException(e.Message, e);
            }
        }

        private bool IsRoleNameExist(List<Role> roles, Command request)
        {
            Role? role = roles
                .FirstOrDefault(chatRole => chatRole.Name == request.role.Name);
            return role is not null;
        }
    }
}