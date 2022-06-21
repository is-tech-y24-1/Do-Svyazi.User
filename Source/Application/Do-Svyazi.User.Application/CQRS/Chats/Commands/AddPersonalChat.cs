using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Commands;

public class AddPersonalChat : IRequest<Guid>
{
    public Guid FirstUserId { get; init; }
    public Guid SecondUserId { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    public class Handler : IRequestHandler<AddPersonalChat, Guid>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<Guid> Handle(AddPersonalChat request, CancellationToken cancellationToken)
        {
            MessengerUser firstUser =
                await _context.Users.SingleOrDefaultAsync(user => user.Id == request.FirstUserId, cancellationToken) ??
                throw new Do_Svyazi_User_NotFoundException(
                    $"User with id = {request.FirstUserId} to create a personal chat was not found");

            MessengerUser secondUser =
                await _context.Users.SingleOrDefaultAsync(user => user.Id == request.SecondUserId, cancellationToken) ??
                throw new Do_Svyazi_User_NotFoundException(
                    $"User with id = {request.SecondUserId} to create a personal chat was not found");

            Chat chat = new PersonalChat(firstUser, secondUser, request.Name, request.Description);

            await _context.Chats.AddAsync(chat, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return chat.Id;
        }
    }
}