using AutoMapper;
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
    {
        MessengerUser messengerUser = await _context.Users
                                          .Include(user => user.Chats)
                                          .SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken: cancellationToken) ??
                                      throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId} to get all user chats");

        var chats = _mapper.Map<IReadOnlyList<MessengerChatDto>>(messengerUser.Chats);

        return chats;
    }

    public async Task<IReadOnlyList<Guid>> Handle(GetAllChatsIdsByUserId request, CancellationToken cancellationToken)
    {
        MessengerUser messengerUser = await _context.Users
                                          .Include(user => user.Chats)
                                          .SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken: cancellationToken) ??
                                      throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId} to get user chat ids");

        var chatIds = messengerUser.Chats.Select(chat => chat.Id).ToList();

        return chatIds;
    }

    public async Task<MessengerUser> Handle(GetUser request, CancellationToken cancellationToken)
    {
        MessengerUser user = await _context.Users
                                 .Include(user => user.Chats)
                                 .SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken: cancellationToken) ??
                             throw new Do_Svyazi_User_NotFoundException($"User with id = {request.userId} doesn't exist");

        return user;
    }

    public async Task<IReadOnlyCollection<MessengerUserDto>> Handle(GetUsers request, CancellationToken cancellationToken)
    {
        List<MessengerUser> users = await _context.Users
            .ToListAsync(cancellationToken: cancellationToken);

        return _mapper.Map<IReadOnlyCollection<MessengerUserDto>>(users);
    }
}