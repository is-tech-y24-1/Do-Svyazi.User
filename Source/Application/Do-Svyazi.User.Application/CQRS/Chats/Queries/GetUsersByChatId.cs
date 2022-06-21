using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

public record GetUsersByChatId : IRequest<IReadOnlyCollection<ChatUser>>
{
    public Guid ChatId { get; init; }

    public class Handler : IRequestHandler<GetUsersByChatId, IReadOnlyCollection<ChatUser>>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<ChatUser>> Handle(GetUsersByChatId request, CancellationToken cancellationToken)
        {
            Chat chat = await _context.Chats
                            .Include(chat => chat.Users)
                                .ThenInclude(user => user.User)
                            .Include(chat => chat.Users)
                                .ThenInclude(user => user.Role)
                            .SingleOrDefaultAsync(chat => chat.Id == request.ChatId, cancellationToken) ??
                        throw new Do_Svyazi_User_NotFoundException($"Chat with id = {request.ChatId} to get users was not found");

            return _mapper.Map<IReadOnlyCollection<ChatUser>>(chat.Users);
        }
    }
}