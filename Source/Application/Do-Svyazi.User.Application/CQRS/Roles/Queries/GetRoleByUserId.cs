using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Roles.Queries;

public class GetRoleByUserId : IRequest<Role>
{
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }

    public class Handler : IRequestHandler<GetRoleByUserId, Role>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<Role> Handle(GetRoleByUserId request, CancellationToken cancellationToken)
        {
            ChatUser chatUser =
                await _context.ChatUsers
                    .Include(chatUser => chatUser.Role)
                    .FirstOrDefaultAsync(user => user.MessengerUserId == request.UserId && user.ChatId == request.ChatId, cancellationToken: cancellationToken) ??
                throw new Do_Svyazi_User_NotFoundException(
                    $"Chat user with userId = {request.UserId} and chatId = {request.ChatId} to get user role was not found");

            return chatUser.Role;
        }
    }
}