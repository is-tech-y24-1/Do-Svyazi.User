using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Roles;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Roles;

public static class ChangeRoleForUserById
{
    public record Command(Guid userId, RoleDto role) : IRequest;

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
            var chatUser = await _context.ChatUsers.FindAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"Can't find user with id = {request.userId}");
            Role? newRole = new Role()
            {
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
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}