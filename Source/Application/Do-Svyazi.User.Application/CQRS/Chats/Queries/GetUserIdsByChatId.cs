using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

public record GetUserIdsByChatId : IRequest<IReadOnlyCollection<Guid>>
{
    public Guid ChatId { get; init; }

    public class Handler : IRequestHandler<GetUserIdsByChatId, IReadOnlyCollection<Guid>>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<Guid>> Handle(GetUserIdsByChatId request, CancellationToken cancellationToken)
        {
            Chat chat = await _context.Chats
                            .Include(chat => chat.Users)
                            .SingleOrDefaultAsync(chat => chat.Id == request.ChatId, cancellationToken) ??
                        throw new Do_Svyazi_User_NotFoundException($"Chat with id = {request.ChatId} to get user ids was not found");

            IReadOnlyCollection<Guid> userIds = chat.Users.Select(user => user.Id).ToList();

            return _mapper.Map<IReadOnlyCollection<Guid>>(userIds);
        }
    }
}
