using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public abstract class Chat
{
    private readonly Role _baseAdminRole = new Role
    {
        CanEdit = ActionOption.Enabled,
        CanDelete = ActionOption.Enabled,
        CanWrite = ActionOption.Enabled,
        CanRead = ActionOption.Enabled,
    };

    private readonly Role _baseUserRole = new Role
    {
        CanEdit = ActionOption.Disabled,
        CanDelete = ActionOption.Enabled,
        CanWrite = ActionOption.Enabled,
        CanRead = ActionOption.Enabled,
    };

    protected Chat() { }

    public Guid Id { get; protected init; } = Guid.NewGuid();
    public string Name { get; init; }
    public string Description { get; init; }

    // public long Tag { get; init; } ??
    public List<ChatUser> Users { get; init; } = new ();
    public List<Role> Roles { get; init; } = new ();
}