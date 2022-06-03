using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetUserIdsByChatId
{
    public record Query(Guid chatId) : IRequest<Response>;
    public record Response(IReadOnlyCollection<Guid> users);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            Chat chat = await _context.Chats
                            .Include(chat => chat.Users)
                            .SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                        throw new Do_Svyazi_User_NotFoundException($"Chat with id {request.chatId} not found");

            IReadOnlyCollection<Guid> result = chat.Users.Select(user => user.Id).ToList();

            return new Response(_mapper.Map<IReadOnlyCollection<Guid>>(result));
        }
    }
}
