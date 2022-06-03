using AutoMapper;
using AutoMapper.QueryableExtensions;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Dtos.Chats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

public static class GetChats
{
    public record Query : IRequest<Response>;
    public record Response(IReadOnlyCollection<MessengerChatDto> chats);

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
            List<MessengerChatDto> chats = await _context
                .Chats
                .ProjectTo<MessengerChatDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken: cancellationToken);

            return new Response(chats);
        }
    }
}