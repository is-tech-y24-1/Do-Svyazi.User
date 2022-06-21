using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public record AddGroupChat : IRequest<Guid>
{
    public Guid AdminId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    public class Handler : IRequestHandler<AddGroupChat, Guid>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<Guid> Handle(AddGroupChat request, CancellationToken cancellationToken)
        {
            MessengerUser user = await _context.Users
                                     .SingleOrDefaultAsync(user => user.Id == request.AdminId, cancellationToken) ??
                                 throw new Do_Svyazi_User_NotFoundException(
                                     $"User with id = {request.AdminId} to create a group chat was not found");

            GroupChat chat = new GroupChat(user, request.Name, request.Description);

            await _context.Chats.AddAsync(chat, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return chat.Id;
        }
    }
}