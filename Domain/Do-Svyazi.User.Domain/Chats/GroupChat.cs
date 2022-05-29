using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class GroupChat : Chat
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

    public new long Id { get; init; }
    public new string Name { get; init; }
    public new string Description { get; init; }

    public new List<ChatUser> Users { get; init; }
    public new List<Role> Roles { get; init; }
}