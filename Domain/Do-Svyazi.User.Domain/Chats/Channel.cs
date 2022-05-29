using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Chats;

public class Channel : Chat
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
        CanDelete = ActionOption.Disabled,
        CanWrite = ActionOption.Disabled,
        CanRead = ActionOption.Enabled,
    };
}