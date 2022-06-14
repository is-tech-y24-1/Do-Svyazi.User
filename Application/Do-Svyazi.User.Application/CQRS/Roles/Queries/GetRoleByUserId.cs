using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Roles;

public static class GetRoleByUserId
{
    public record Command(Guid userId, Guid chatId) : IRequest<Role>;

    public class Handler : IRequestHandler<Command, Role>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Role> Handle(Command request, CancellationToken cancellationToken)
        {
            ChatUser? chatUser = await _context.ChatUsers
                                     .Include(chatUser => chatUser.Role)
                                     .FirstOrDefaultAsync(
                                         user => user.User.Id == request.userId
                                                 && user.ChatId == request.chatId,
                                         cancellationToken: cancellationToken) ??
                                 throw new Do_Svyazi_User_NotFoundException(
                                     $"Chat user with userId = {request.userId} and chatId = {request.chatId} not found");
            return chatUser.Role;
        }
    }
}