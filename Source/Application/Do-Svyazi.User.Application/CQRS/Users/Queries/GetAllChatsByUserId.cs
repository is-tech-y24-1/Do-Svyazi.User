using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Queries;

public class GetAllChatsByUserId : IRequest<IReadOnlyList<MessengerChatDto>>
{
    public Guid UserId { get; init; }

    public class Handler : IRequestHandler<GetAllChatsByUserId, IReadOnlyList<MessengerChatDto>>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<MessengerChatDto>> Handle(GetAllChatsByUserId request, CancellationToken cancellationToken)
        {
            MessengerUser messengerUser = await _context.Users.FindAsync(request.UserId) ??
                                          throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.UserId} to get all user chats");

            var chats = _mapper.Map<IReadOnlyList<MessengerChatDto>>(messengerUser.Chats);

            return chats;
        }
    }
}