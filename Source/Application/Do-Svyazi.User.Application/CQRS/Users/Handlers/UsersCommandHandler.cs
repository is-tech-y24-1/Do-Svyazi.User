using Do_Svyazi.User.Application.CQRS.Handlers;
using Do_Svyazi.User.Application.CQRS.Users.Commands;
using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Handlers;

public class UsersCommandHandler :
    IQueryHandler<AddUser, Guid>,
    IQueryHandler<ChangeUserDescriptionById, Unit>,
    IQueryHandler<ChangeUserNameById, Unit>,
    IQueryHandler<SetUserNickNameById, Unit>,
    IQueryHandler<DeleteUser, Unit>
{
    private readonly IDbContext _context;
    public UsersCommandHandler(IDbContext context) => _context = context;

    public async Task<Guid> Handle(AddUser request, CancellationToken cancellationToken)
    {
        if (NickNameExists(request.nickName))
        {
            throw new Do_Svyazi_User_BusinessLogicException(
                $"User with nickname = {request.nickName} already exists in messenger");
        }

        MessengerUser messengerUser = new (request.name, request.nickName, request.description);

        await _context.Users.AddAsync(messengerUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return messengerUser.Id;
    }

    public async Task<Unit> Handle(ChangeUserDescriptionById request, CancellationToken cancellationToken)
    {
        MessengerUser messengerUser = await _context.Users.FindAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"User with id {request.userId} to change description was not found");

        messengerUser.ChangeDescription(request.description);

        _context.Users.Update(messengerUser);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    public async Task<Unit> Handle(ChangeUserNameById request, CancellationToken cancellationToken)
    {
        MessengerUser messengerUser = await _context.Users.FindAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException($"User with id {request.userId} to change name was not found");

        messengerUser.ChangeName(request.name);

        _context.Users.Update(messengerUser);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    public async Task<Unit> Handle(SetUserNickNameById request, CancellationToken cancellationToken)
    {
        MessengerUser messengerUser = await _context.Users.FindAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException(
                                          $"User with id {request.userId} to change nickName was not found");

        if (NickNameExists(request.nickName))
            throw new Do_Svyazi_User_BusinessLogicException($"Nickname = {request.nickName} already exists in messenger");

        messengerUser.ChangeNickName(request.nickName);

        _context.Users.Update(messengerUser);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    public async Task<Unit> Handle(DeleteUser request, CancellationToken cancellationToken)
    {
        MessengerUser messengerUser = await _context.Users.FindAsync(request.userId) ??
                                      throw new Do_Svyazi_User_NotFoundException($"Can't find user with id = {request.userId} to delete");

        _context.Users.Remove(messengerUser);
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }

    private bool NickNameExists(string nickName) => _context.Users.Any(user => user.NickName == nickName);
}