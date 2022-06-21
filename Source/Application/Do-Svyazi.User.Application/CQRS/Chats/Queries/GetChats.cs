using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Dtos.Chats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

public record GetChats : IRequest<IReadOnlyCollection<MessengerChatDto>>
{
    public class Handler : IRequestHandler<GetChats, IReadOnlyCollection<MessengerChatDto>>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyCollection<MessengerChatDto>> Handle(GetChats request, CancellationToken cancellationToken)
        {
            List<Chat> chats = await _context.Chats.ToListAsync(cancellationToken: cancellationToken);

            return _mapper.Map<IReadOnlyCollection<MessengerChatDto>>(chats);
        }
    }
}