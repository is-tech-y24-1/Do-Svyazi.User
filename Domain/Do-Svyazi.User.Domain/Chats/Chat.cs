using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public abstract class Chat
{
    private readonly List<ChatUser> _users = new ();
    private readonly List<Role> _roles = new ();

    protected Chat() { }

    protected Chat(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public Guid Id { get; protected init; } = Guid.NewGuid();
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    // public long Tag { get; init; } ??
    public IReadOnlyCollection<ChatUser> Users => _users.AsReadOnly();
    public IReadOnlyCollection<Role> Roles => _roles.AsReadOnly();

    protected Role BaseAdminRole { get; init; }
    protected Role BaseUserRole { get; init; }

    public virtual void ChangeNameChat(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException("Chat name to set is null");

        Name = name;
    }

    public virtual void ChangeDescriptionChat(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException("Chat name to set is null");

        Description = description;
    }

    public List<ChatUser> GetChatUsersRole(Role role)
    {
        return Users.Where(user => user.Role.Equals(role)).ToList();
    }

    public ChatUser GetChatUser(string nickName)
    {
        return Users.SingleOrDefault(user => user.NickName == nickName)
               ?? throw new Do_Svyazi_User_NotFoundException("User is not found in chat");
    }

    public virtual void AddChatUser(ChatUser chatUser)
    {
        if (chatUser is null)
            throw new ArgumentNullException("User to set is null");

        if (Users.Contains(chatUser))
            throw new Do_Svyazi_User_InnerLogicException("This user already exists");

        _users.Add(chatUser);
    }

    public virtual void RemoveChatUser(ChatUser chatUser)
    {
        if (chatUser is null)
            throw new ArgumentNullException("User to set is null");

        if (!_users.Remove(chatUser))
            throw new Exception("This user doesn't exist in this chat");
    }

    public virtual void AddRole(Role role)
    {
        if (role is null)
            throw new ArgumentNullException("User to set is null");

        if (Roles.Contains(role))
            throw new Do_Svyazi_User_InnerLogicException("This role already exists");

        _roles.Add(role);
    }

    public virtual void RemoveRole(Role role)
    {
        if (role is null)
            throw new ArgumentNullException("Role to set is null");

        if (!_roles.Remove(role))
            throw new Exception("This role doesn't exist in this chat");
    }
}