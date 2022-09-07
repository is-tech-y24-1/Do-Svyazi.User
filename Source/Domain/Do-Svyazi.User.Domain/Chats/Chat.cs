using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public abstract class Chat
{
    protected Chat() { }

    protected Chat(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Chat name to set is null");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException(nameof(description), "Chat description to set is null");

        Name = name;
        Description = description;
    }

    public Guid Id { get; protected init; } = Guid.NewGuid();
    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public int MaxUsersAmount { get; init; }
    public List<ChatUser> Users { get; init; } = new ();
    public List<Role> Roles { get; init; } = new ();

    protected Role BaseAdminRole { get; init; }
    protected Role BaseUserRole { get; init; }

    public virtual void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), $"Chat name to change in chat {Name} is null");

        Name = name;
    }

    public virtual void ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException(nameof(description), $"Chat description to set in chat {Name} is null");

        Description = description;
    }

    public IReadOnlyCollection<ChatUser> GetUsersByRole(Role role) =>
        Users.Where(user => user.Role.Equals(role)).ToList();

    public ChatUser GetUser(string nickName) =>
        Users.SingleOrDefault(user => user.User.NickName == nickName)
        ?? throw new Do_Svyazi_User_NotFoundException($"User is not found in chat {Name}");

    public ChatUser GetUser(Guid id) =>
        Users.SingleOrDefault(user => user.MessengerUserId == id)
        ?? throw new Do_Svyazi_User_NotFoundException($"User is not found in chat {Name}");

    public abstract ChatUser AddUser(MessengerUser user);
    public abstract ChatUser RemoveUser(MessengerUser user);
    public abstract void AddRole(Role role);
    public abstract void RemoveRole(Role role);
    public abstract void ChangeUserRole(MessengerUser user, Role role);

    public bool IsUserExist(MessengerUser messengerUser) =>
        Users.Any(user => user.MessengerUserId == messengerUser.Id);

    protected ChatUser CreateChatUser(MessengerUser user, Role role) => new (user, this, role);
}