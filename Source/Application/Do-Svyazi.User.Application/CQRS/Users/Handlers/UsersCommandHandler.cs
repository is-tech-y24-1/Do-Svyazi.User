using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Do_Svyazi.User.Application.CQRS.Users.Handlers;

public class UsersCommandHandler :
    IQueryHandler<ChangeUserDescriptionByIdCommand, Unit>,
    IQueryHandler<ChangeUserNameByIdCommand, Unit>,
    IQueryHandler<SetUserNickNameByIdCommand, Unit>,
    IQueryHandler<DeleteUserCommand, Unit>
{
    private readonly UserManager<MessengerUser> _userManager;
    public UsersCommandHandler(UserManager<MessengerUser> userManager) => _userManager = userManager;

    public async Task<Unit> Handle(ChangeUserDescriptionByIdCommand request, CancellationToken cancellationToken)
    {
        MessengerUser messengerUser = await _userManager.FindByIdAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"User with id {request.userId} to change description was not found");

        messengerUser.ChangeDescription(request.description);

        await _userManager.UpdateAsync(messengerUser);

        return Unit.Value;
    }

    public async Task<Unit> Handle(ChangeUserNameByIdCommand request, CancellationToken cancellationToken)
    {
        var users = _userManager.Users.ToList();

        MessengerUser messengerUser = await _userManager.FindByIdAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException($"User with id {request.userId} to change name was not found");

        messengerUser.ChangeName(request.name);

        await _userManager.UpdateAsync(messengerUser);

        return Unit.Value;
    }

    public async Task<Unit> Handle(SetUserNickNameByIdCommand request, CancellationToken cancellationToken)
    {
        MessengerUser messengerUser = await _userManager.FindByIdAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"User with id {request.userId} to change nickName was not found");

        if (NickNameExists(request.nickName))
            throw new Do_Svyazi_User_BusinessLogicException($"Nickname = {request.nickName} already exists in messenger");

        messengerUser.ChangeNickName(request.nickName);

        await _userManager.UpdateAsync(messengerUser);

        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        MessengerUser messengerUser = await _userManager.FindByIdAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId} to delete");

        await _userManager.DeleteAsync(messengerUser);

        return Unit.Value;
    }

    private bool NickNameExists(string nickName) => _userManager.FindByNameAsync(nickName) is not null;
}