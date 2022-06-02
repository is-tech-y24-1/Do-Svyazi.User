using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Users;

namespace Do_Svyazi.User.Application.CQRS.Chats.Queries;

using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using MediatR;
using Microsoft.EntityFrameworkCore;

public static class GetUsersToChat
{
    public record Query(Guid chatId) : IRequest<Response>;
    public record Response(IReadOnlyCollection<MessengerUserDto> users);

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
            Chat chat = await _context.Chats.SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                throw new Do_Svyazi_User_NotFoundException($"Can't find chat with id = {request.chatId}");

            IReadOnlyCollection<ChatUser> result = chat.Users;

            return new Response(_mapper.Map<IReadOnlyCollection<MessengerUserDto>>(result));
        }
    }
}
