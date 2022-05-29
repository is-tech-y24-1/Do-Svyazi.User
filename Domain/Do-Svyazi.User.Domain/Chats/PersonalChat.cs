using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Chats;

public class PersonalChat : Chat
{
    private readonly Role _baseAdminRole = new Role
    {
        CanEditMessage = ActionOption.Enabled,
        CanDeleteMessage = ActionOption.Enabled,
        CanWriteMessage = ActionOption.Enabled,
        CanReadMessage = ActionOption.Enabled,
        CanAddUsers = ActionOption.Unavailable,
        CanDeleteUsers = ActionOption.Unavailable,
        CanPinMessages = ActionOption.Enabled,
        CanSeeChannelMembers = ActionOption.Enabled,
        CanInviteOtherUsers = ActionOption.Unavailable,
        CanEditChannelDescription = ActionOption.Unavailable,
        CanDeleteChat = ActionOption.Enabled,
    };

    private readonly Role _baseUserRole = new Role
    {
        CanEditMessage = ActionOption.Enabled,
        CanDeleteMessage = ActionOption.Enabled,
        CanWriteMessage = ActionOption.Enabled,
        CanReadMessage = ActionOption.Enabled,
        CanAddUsers = ActionOption.Unavailable,
        CanDeleteUsers = ActionOption.Unavailable,
        CanPinMessages = ActionOption.Enabled,
        CanSeeChannelMembers = ActionOption.Enabled,
        CanInviteOtherUsers = ActionOption.Unavailable,
        CanEditChannelDescription = ActionOption.Unavailable,
        CanDeleteChat = ActionOption.Enabled,
    };

    protected PersonalChat()
    {
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;
    }
}