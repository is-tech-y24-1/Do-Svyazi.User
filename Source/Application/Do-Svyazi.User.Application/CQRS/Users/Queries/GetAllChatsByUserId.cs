using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public static class GetAllChatsByUserId
{
    public record Query(Guid userId) : IRequest<Response>;
    public record Response(IReadOnlyList<MessengerChatDto> users);

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
            MessengerUser messengerUser = await _context.Users.FindAsync(request.userId) ??
                                          throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId} to get all user chats");

            var chats = _mapper.Map<IReadOnlyList<MessengerChatDto>>(messengerUser.Chats);

            return new Response(chats);
        }
    }
}