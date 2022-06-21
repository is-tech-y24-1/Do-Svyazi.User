using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Users;
using MediatR;

namespace Do_Svyazi.User.Application.CQRS.Users.Commands;

public class AddUser : IRequest<Guid>
{
    public string Name { get; init; }
    public string NickName { get; init; }
    public string Description { get; init; }

    public class Handler : IRequestHandler<AddUser, Guid>
    {
        private readonly IDbContext _context;
        public Handler(IDbContext context) => _context = context;

        public async Task<Guid> Handle(AddUser request, CancellationToken cancellationToken)
        {
            if (IsUserExist(request.NickName))
            {
                throw new Do_Svyazi_User_BusinessLogicException(
                    $"User with nickname = {request.NickName} already exists in messenger");
            }

            MessengerUser messengerUser = new (request.Name, request.NickName, request.Description);

            await _context.Users.AddAsync(messengerUser, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return messengerUser.Id;
        }

        private bool IsUserExist(string nickName) =>
            _context.Users.Any(user => user.NickName == nickName);
    }
}