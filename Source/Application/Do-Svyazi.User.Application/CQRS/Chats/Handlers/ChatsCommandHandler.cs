using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Handlers;

public class ChatsCommandHandler :
    ICommandHandler<AddChannel, Guid>,
    ICommandHandler<AddGroupChat, Guid>,
    ICommandHandler<AddPersonalChat, Guid>,
    ICommandHandler<AddSavedMessages, Guid>,
    ICommandHandler<AddUserToChat, Unit>,
    ICommandHandler<DeleteUserFromChat, Unit>
{
    private readonly IDbContext _context;

    public ChatsCommandHandler(IDbContext context) => _context = context;

    public async Task<Guid> Handle(AddChannel request, CancellationToken cancellationToken)
    {
        MessengerUser user = await _context.Users
                                 .SingleOrDefaultAsync(user => user.Id == request.adminId, cancellationToken) ??
                             throw new Do_Svyazi_User_NotFoundException(
                                 $"User with id = {request.adminId} to create a channel was not found");

        Chat chat = new Channel(user, request.name, request.description);
        var chatAdmin = chat.Users.Single();

        await _context.ChatUsers.AddAsync(chatAdmin, cancellationToken);
        await _context.Chats.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return chat.Id;
    }

    public async Task<Guid> Handle(AddGroupChat request, CancellationToken cancellationToken)
    {
        MessengerUser user = await _context.Users
                                 .SingleOrDefaultAsync(user => user.Id == request.adminId, cancellationToken) ??
                             throw new Do_Svyazi_User_NotFoundException(
                                 $"User with id = {request.adminId} to create a group chat was not found");

        GroupChat chat = new GroupChat(user, request.name, request.description);
        var chatAdmin = chat.Users.Single();

        await _context.ChatUsers.AddAsync(chatAdmin, cancellationToken);
        await _context.Chats.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return chat.Id;
    }

    public async Task<Guid> Handle(AddPersonalChat request, CancellationToken cancellationToken)
    {
        MessengerUser firstUser =
            await _context.Users.SingleOrDefaultAsync(user => user.Id == request.firstUserId, cancellationToken) ??
            throw new Do_Svyazi_User_NotFoundException(
                $"User with id = {request.firstUserId} to create a personal chat was not found");

        MessengerUser secondUser =
            await _context.Users.SingleOrDefaultAsync(user => user.Id == request.secondUserId, cancellationToken) ??
            throw new Do_Svyazi_User_NotFoundException(
                $"User with id = {request.secondUserId} to create a personal chat was not found");

        Chat chat = new PersonalChat(firstUser, secondUser, request.name, request.description);

        _context.Users.Update(firstUser);
        _context.Users.Update(secondUser);
        await _context.Chats.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return chat.Id;
    }

    public async Task<Guid> Handle(AddSavedMessages request, CancellationToken cancellationToken)
    {
        MessengerUser user =
            await _context.Users.SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken) ??
            throw new Do_Svyazi_User_NotFoundException(
                $"User with id = {request.userId} to create saved messages chat not found");

        Chat chat = new SavedMessages(user, request.name, request.description);
        var chatAdmin = chat.Users.Single();

        await _context.ChatUsers.AddAsync(chatAdmin, cancellationToken);
        await _context.Chats.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return chat.Id;
    }

    public async Task<Unit> Handle(AddUserToChat request, CancellationToken cancellationToken)
    {
        Chat chat = await _context.Chats
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.User)
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.Role)
                        .SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                    throw new Do_Svyazi_User_NotFoundException(
                        $"Chat with id = {request.chatId} to add user {request.userId} was not found");

        MessengerUser messengerUser = await _context.Users
                                          .SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"User with id = {request.userId} to be added into chat with id = {request.chatId} not found");

        ChatUser newChatUser = chat.AddUser(messengerUser);

        await _context.ChatUsers.AddAsync(newChatUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteUserFromChat request, CancellationToken cancellationToken)
    {
        Chat chat = await _context.Chats
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.User)
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.Role)
                        .SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                    throw new Do_Svyazi_User_NotFoundException($"Chat with id {request.chatId} not found");

        MessengerUser messengerUser = await _context.Users
                                          .SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"User with id {request.userId} not found");

        var removedUser = chat.RemoveUser(messengerUser);

        // TODO: debug, if chat removes from user's List<Chat> property
        _context.ChatUsers.Remove(removedUser);
        _context.Chats.Update(chat);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}