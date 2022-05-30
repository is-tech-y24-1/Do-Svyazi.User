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

    // public long Tag { get; init; } ??
    public IReadOnlyCollection<ChatUser> GetUsers => Users;
    public IReadOnlyCollection<Role> GetRoles => Roles;

    protected Role BaseAdminRole { get; init; }
    protected Role BaseUserRole { get; init; }
    protected List<ChatUser> Users { get; } = new ();
    protected List<Role> Roles { get; } = new ();

    public virtual void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Chat name to set is null");

        Name = name;
    }

    public virtual void ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException(nameof(description), "Chat description to set is null");

        Description = description;
    }

    public IReadOnlyCollection<ChatUser> GetUsersRole(Role role) =>
        GetUsers.Where(user => user.Role.Equals(role)).ToList();

    public ChatUser GetUser(string nickName) =>
        GetUsers.SingleOrDefault(user => user.User.NickName == nickName)
        ?? throw new Do_Svyazi_User_NotFoundException($"User is not found in chat {Name}");

    public abstract void AddUser(MessengerUser messengerUser);
    public abstract void RemoveUser(MessengerUser messengerUser);
    public abstract void AddRole(Role role);
    public abstract void RemoveRole(Role role);

    protected ChatUser CreateUser(MessengerUser messengerUser, Chat chat, Role role)
    {
        var user = new ChatUser
        {
            Role = role,
            Chat = chat,
            User = messengerUser,
        };

        return user;
    }
}