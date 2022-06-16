using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Roles.Queries;

public static class GetRoleByUserId
{
    public record Query(Guid userId, Guid chatId) : IRequest<Response>;
    public record Response(Role userRole);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IDbContext _context;

        public Handler(IDbContext context) => _context = context;

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            ChatUser chatUser =
                await _context.ChatUsers
                    .Include(chatUser => chatUser.Role)
                    .FirstOrDefaultAsync(user => user.MessengerUserId == request.userId && user.ChatId == request.chatId, cancellationToken: cancellationToken) ??
                throw new Do_Svyazi_User_NotFoundException(
                    $"Chat user with userId = {request.userId} and chatId = {request.chatId} to get user role was not found");

            return new Response(chatUser.Role);
        }
    }
}