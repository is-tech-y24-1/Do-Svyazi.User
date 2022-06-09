using AutoMapper;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public static class GetAllChatsByUserId
{
    public record Command(Guid userId) : IRequest<IReadOnlyList<MessengerChatDto>>;

    public class Handler : IRequestHandler<Command, IReadOnlyList<MessengerChatDto>>
    {
        private readonly IDbContext _context;
        private readonly IMapper _mapper;

        public Handler(IDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<MessengerChatDto>> Handle(Command request, CancellationToken cancellationToken)
        {
            MessengerUser? messengerUser = await _context.Users.FindAsync(request.userId) ??
                                           throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId}");
            return _mapper.Map<IReadOnlyList<MessengerChatDto>>(messengerUser.Chats);
        }
    }
}