using AutoMapper;
using Do_Svyazi.User.Application.CQRS.Chats.Queries;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using Do_Svyazi.User.Dtos.Chats;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Handlers;

public class ChatsQueryHandler :
    IQueryHandler<GetChatByIdQuery, MessengerChatDto>,
    IQueryHandler<GetChatsQuery, IReadOnlyCollection<MessengerChatDto>>,
    IQueryHandler<GetUserIdsByChatIdQuery, IReadOnlyCollection<Guid>>,
    IQueryHandler<GetUsersByChatIdQuery, IReadOnlyCollection<ChatUser>>
{
    private readonly IDbContext _context;
    private readonly IMapper _mapper;

    public ChatsQueryHandler(IDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<MessengerChatDto> Handle(GetChatByIdQuery request, CancellationToken cancellationToken)
    {
        Chat chat = await _context.Chats
                        .Include(chat => chat.Users)
                        .Include(chat => chat.Roles)
                        .SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                    throw new Do_Svyazi_User_NotFoundException($"Chat with id = {request.chatId} was not found");

        return _mapper.Map<MessengerChatDto>(chat);
    }

    public async Task<IReadOnlyCollection<MessengerChatDto>> Handle(GetChatsQuery request, CancellationToken cancellationToken)
    {
        List<Chat> chats = await _context.Chats
            .Include(chat => chat.Users)
            .Include(chat => chat.Roles)
            .ToListAsync(cancellationToken: cancellationToken);

        return _mapper.Map<IReadOnlyCollection<MessengerChatDto>>(chats);
    }

    public async Task<IReadOnlyCollection<Guid>> Handle(GetUserIdsByChatIdQuery request, CancellationToken cancellationToken)
    {
        Chat chat = await _context.Chats
                        .Include(chat => chat.Users)
                        .SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                    throw new Do_Svyazi_User_NotFoundException($"Chat with id = {request.chatId} to get user ids was not found");

        IReadOnlyCollection<Guid> userIds = chat.Users.Select(user => user.Id).ToList();

        return _mapper.Map<IReadOnlyCollection<Guid>>(userIds);
    }

    public async Task<IReadOnlyCollection<ChatUser>> Handle(GetUsersByChatIdQuery request, CancellationToken cancellationToken)
    {
        Chat chat = await _context.Chats
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.User)
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.Role)
                        .SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                    throw new Do_Svyazi_User_NotFoundException($"Chat with id = {request.chatId} to get users was not found");

        return _mapper.Map<IReadOnlyCollection<ChatUser>>(chat.Users);
    }
}