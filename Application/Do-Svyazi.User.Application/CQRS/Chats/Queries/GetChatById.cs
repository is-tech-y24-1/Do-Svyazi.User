using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Dtos.Chats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

public static class GetChatById
{
    public record Query(Guid chatId) : IRequest<Response>;
    public record Response(MessengerChatDto chat);

    public class Handler : IRequestHandler<Query, Response>
    {
        private readonly IUsersAndChatDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IUsersAndChatDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response> Handle(Query request, CancellationToken cancellationToken)
        {
            var result = await _context.Chats
                .FirstOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken);

            return new Response(_mapper.Map<MessengerChatDto>(result));
        }
    }
}