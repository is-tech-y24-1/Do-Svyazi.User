using Do_Svyazi.User.Application.CQRS.Chats.Commands;
using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.CQRS.Chats.Handlers;

public class ChatsCommandHandler :
    ICommandHandler<AddChannelCommand, Guid>,
    ICommandHandler<AddGroupChatCommand, Guid>,
    ICommandHandler<AddPersonalChatCommand, Guid>,
    ICommandHandler<AddSavedMessagesCommand, Guid>,
    ICommandHandler<AddUserToChatCommand, Unit>,
    ICommandHandler<DeleteUserFromChatCommand, Unit>
{
    private readonly IDbContext _context;
    private readonly UserManager<MessengerUser> _userManager;

    public ChatsCommandHandler(UserManager<MessengerUser> userManager, IDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<Guid> Handle(AddChannelCommand request, CancellationToken cancellationToken)
    {
        MessengerUser user = await _userManager.Users
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

    public async Task<Guid> Handle(AddGroupChatCommand request, CancellationToken cancellationToken)
    {
        MessengerUser user = await _userManager.FindByIdAsync($"{request.adminId}")
                             ?? throw new Do_Svyazi_User_NotFoundException(
                                 $"User with id = {request.adminId} to create a group chat was not found");

        GroupChat chat = new GroupChat(user, request.name, request.description);
        var chatAdmin = chat.Users.Single();

        await _context.ChatUsers.AddAsync(chatAdmin, cancellationToken);
        await _context.Chats.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return chat.Id;
    }

    public async Task<Guid> Handle(AddPersonalChatCommand request, CancellationToken cancellationToken)
    {
        MessengerUser firstUser =
            await _userManager.Users.SingleOrDefaultAsync(user => user.Id == request.firstUserId, cancellationToken) ??
            throw new Do_Svyazi_User_NotFoundException(
                $"User with id = {request.firstUserId} to create a personal chat was not found");

        MessengerUser secondUser =
            await _userManager.Users.SingleOrDefaultAsync(user => user.Id == request.secondUserId, cancellationToken) ??
            throw new Do_Svyazi_User_NotFoundException(
                $"User with id = {request.secondUserId} to create a personal chat was not found");

        Chat chat = new PersonalChat(firstUser, secondUser, request.name, request.description);

        await _context.ChatUsers.AddRangeAsync(chat.Users, cancellationToken);
        await _context.Chats.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return chat.Id;
    }

    public async Task<Guid> Handle(AddSavedMessagesCommand request, CancellationToken cancellationToken)
    {
        MessengerUser user =
            await _userManager.Users.SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken) ??
            throw new Do_Svyazi_User_NotFoundException(
                $"User with id = {request.userId} to create saved messages chat not found");

        Chat chat = new SavedMessages(user, request.name, request.description);
        var chatAdmin = chat.Users.Single();

        await _context.ChatUsers.AddAsync(chatAdmin, cancellationToken);
        await _context.Chats.AddAsync(chat, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return chat.Id;
    }

    public async Task<Unit> Handle(AddUserToChatCommand request, CancellationToken cancellationToken)
    {
        Chat chat = await _context.Chats
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.User)
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.Role)
                        .SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                    throw new Do_Svyazi_User_NotFoundException(
                        $"Chat with id = {request.chatId} to add user {request.userId} was not found");

        MessengerUser messengerUser = await _userManager.Users
                                          .SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"User with id = {request.userId} to be added into chat with id = {request.chatId} not found");

        ChatUser newChatUser = chat.AddUser(messengerUser);

        await _context.ChatUsers.AddAsync(newChatUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteUserFromChatCommand request, CancellationToken cancellationToken)
    {
        Chat chat = await _context.Chats
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.User)
                        .Include(chat => chat.Users)
                        .ThenInclude(user => user.Role)
                        .SingleOrDefaultAsync(chat => chat.Id == request.chatId, cancellationToken) ??
                    throw new Do_Svyazi_User_NotFoundException($"Chat with id {request.chatId} not found");

        MessengerUser messengerUser = await _userManager.Users
                                          .SingleOrDefaultAsync(user => user.Id == request.userId, cancellationToken) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"User with id {request.userId} not found");

        var removedUser = chat.RemoveUser(messengerUser);

        _context.ChatUsers.Remove(removedUser);
        _context.Chats.Update(chat);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}