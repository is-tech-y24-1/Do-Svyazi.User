using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class GroupChat : Chat
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
        CanSeeChannelMembers = ActionOption.Enabled,
        CanInviteOtherUsers = ActionOption.Enabled,
        CanEditChannelDescription = ActionOption.Enabled,
        CanDeleteChat = ActionOption.Enabled,
    };

    private readonly Role _baseUserRole = new Role
    {
        CanEditMessage = ActionOption.Disabled,
        CanDeleteMessage = ActionOption.Enabled,
        CanWriteMessage = ActionOption.Enabled,
        CanReadMessage = ActionOption.Enabled,
        CanAddUsers = ActionOption.Disabled,
        CanDeleteUsers = ActionOption.Disabled,
        CanPinMessages = ActionOption.Enabled,
        CanSeeChannelMembers = ActionOption.Enabled,
        CanInviteOtherUsers = ActionOption.Enabled,
        CanEditChannelDescription = ActionOption.Disabled,
        CanDeleteChat = ActionOption.Disabled,
    };

    protected GroupChat()
    {
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;
    }
}