using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using Do_Svyazi.User.Dtos.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public class GetAllChatsByUserId
{
    public record Command(Guid userId) : IRequest<List<MessengerChatDto>>;

    public class Handler : IRequestHandler<Command, List<MessengerChatDto>>
    {
        private readonly IUsersAndChatDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IUsersAndChatDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MessengerChatDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser? messengerUser = await _context.Users.FindAsync(request.userId) ??
                                           throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId}");

            return messengerUser.Chats.Select(chat => _mapper.Map<MessengerChatDto>(chat)).ToList();
        }
    }
}