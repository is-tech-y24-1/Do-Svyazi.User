using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Chats;

public class Channel : Chat
{
    private readonly Role _baseAdminRole = new Role
    {
        CanEditMessage = ActionOption.Enabled,
        CanDeleteMessage = ActionOption.Enabled,
        CanWriteMessage = ActionOption.Enabled,
        CanReadMessage = ActionOption.Enabled,
        CanAddUsers = ActionOption.Enabled,
        CanDeleteUsers = ActionOption.Enabled,
        CanPinMessages = ActionOption.Enabled,
        CanInviteOtherUsers = ActionOption.Unavailable,
        CanEditChannelDescription = ActionOption.Enabled,
        CanDeleteChat = ActionOption.Enabled,
    };

    private readonly Role _baseUserRole = new Role
    {
        CanEditMessage = ActionOption.Disabled,
        CanDeleteMessage = ActionOption.Disabled,
        CanWriteMessage = ActionOption.Disabled,
        CanReadMessage = ActionOption.Enabled,
        CanAddUsers = ActionOption.Disabled,
        CanDeleteUsers = ActionOption.Disabled,
        CanPinMessages = ActionOption.Disabled,
        CanInviteOtherUsers = ActionOption.Enabled,
        CanEditChannelDescription = ActionOption.Disabled,
        CanDeleteChat = ActionOption.Disabled,
    };
    protected Channel()
    {
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;
    }
}