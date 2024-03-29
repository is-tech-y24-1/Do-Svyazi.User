using AutoMapper;
using AutoMapper.QueryableExtensions;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.CQRS.Users.Queries;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using Do_Svyazi.User.Dtos.Users;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Users.Handlers;

public class UsersQueryHandler :
    IQueryHandler<GetAllChatsByUserId, IReadOnlyList<MessengerChatDto>>,
    IQueryHandler<GetAllChatsIdsByUserId, IReadOnlyList<Guid>>,
    IQueryHandler<GetUser, MessengerUser>,
    IQueryHandler<GetUsers, IReadOnlyCollection<MessengerUserDto>>
{
    private readonly IDbContext _context;
    private readonly IMapper _mapper;

    public UsersQueryHandler(IDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<MessengerChatDto>> Handle(GetAllChatsByUserId request, CancellationToken cancellationToken)
        => await _context.ChatUsers
            .Include(user => user.Chat)
            .Where(user => user.MessengerUserId == request.userId)
            .Select(chatUser => chatUser.Chat)
            .ProjectTo<MessengerChatDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Guid>> Handle(GetAllChatsIdsByUserId request, CancellationToken cancellationToken)
        => await _context.ChatUsers
            .Where(user => user.MessengerUserId == request.userId)
            .Select(chatUser => chatUser.ChatId)
            .ToListAsync(cancellationToken);

    public async Task<MessengerUser> Handle(GetUser request, CancellationToken cancellationToken)
        => await _context.Users
               .SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken: cancellationToken) ??
           throw new Do_Svyazi_User_NotFoundException($"User with id = {request.userId} doesn't exist");

    public async Task<IReadOnlyCollection<MessengerUserDto>> Handle(GetUsers request, CancellationToken cancellationToken)
        => await _context.Users
            .ProjectTo<MessengerUserDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
}