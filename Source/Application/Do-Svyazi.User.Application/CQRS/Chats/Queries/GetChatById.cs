using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Dtos.Chats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

public record GetChatById : IRequest<MessengerChatDto>
{
    public Guid ChatId { get; init; }

    public class Handler : IRequestHandler<GetChatById, MessengerChatDto>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MessengerChatDto> Handle(GetChatById request, CancellationToken cancellationToken)
        {
            Chat chat = await _context.Chats
                            .Include(chat => chat.Creator)
                            .Include(chat => chat.Users)
                            .SingleOrDefaultAsync(chat => chat.Id == request.ChatId, cancellationToken) ??
                        throw new Do_Svyazi_User_NotFoundException($"Chat with id = {request.ChatId} was not found");

            return _mapper.Map<MessengerChatDto>(chat);
        }
    }
}