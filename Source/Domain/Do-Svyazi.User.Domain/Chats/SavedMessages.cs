using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class SavedMessages : Chat
{
    private readonly int _maxUsersAmount = 1;
    private readonly Role _baseAdminRole;
    private readonly Role _baseUserRole;

    public SavedMessages(MessengerUser messengerUser, string name, string description)
        : base(name, description)
    {
        MaxUsersAmount = _maxUsersAmount;

        _baseAdminRole = new Role
        {
            Name = "admin",
            CanEditMessages = ActionOption.Enabled,
            CanDeleteMessages = ActionOption.Enabled,
            CanWriteMessages = ActionOption.Enabled,
            CanReadMessages = ActionOption.Enabled,
            CanAddUsers = ActionOption.Unavailable,
            CanDeleteUsers = ActionOption.Unavailable,
            CanPinMessages = ActionOption.Enabled,
            CanSeeChannelMembers = ActionOption.Unavailable,
            CanInviteOtherUsers = ActionOption.Unavailable,
            CanEditChannelDescription = ActionOption.Unavailable,
            CanDeleteChat = ActionOption.Unavailable,
            Chat = this,
        };

        _baseUserRole = new Role
        {
            Name = "base",
            CanEditMessages = ActionOption.Enabled,
            CanDeleteMessages = ActionOption.Enabled,
            CanWriteMessages = ActionOption.Enabled,
            CanReadMessages = ActionOption.Enabled,
            CanAddUsers = ActionOption.Unavailable,
            CanDeleteUsers = ActionOption.Unavailable,
            CanPinMessages = ActionOption.Enabled,
            CanSeeChannelMembers = ActionOption.Unavailable,
            CanInviteOtherUsers = ActionOption.Unavailable,
            CanEditChannelDescription = ActionOption.Unavailable,
            CanDeleteChat = ActionOption.Unavailable,
            Chat = this,
        };
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;

        ChatUser user = CreateChatUser(messengerUser, _baseAdminRole);
        Users.Add(user);
    }

    protected SavedMessages()
    {
        _baseUserRole = new Role
        {
            Name = "base",
            CanEditMessages = ActionOption.Enabled,
            CanDeleteMessages = ActionOption.Enabled,
            CanWriteMessages = ActionOption.Enabled,
            CanReadMessages = ActionOption.Enabled,
            CanAddUsers = ActionOption.Unavailable,
            CanDeleteUsers = ActionOption.Unavailable,
            CanPinMessages = ActionOption.Enabled,
            CanSeeChannelMembers = ActionOption.Unavailable,
            CanInviteOtherUsers = ActionOption.Unavailable,
            CanEditChannelDescription = ActionOption.Unavailable,
            CanDeleteChat = ActionOption.Unavailable,
            Chat = this,
        };
        _baseAdminRole = new Role
        {
            Name = "admin",
            CanEditMessages = ActionOption.Enabled,
            CanDeleteMessages = ActionOption.Enabled,
            CanWriteMessages = ActionOption.Enabled,
            CanReadMessages = ActionOption.Enabled,
            CanAddUsers = ActionOption.Unavailable,
            CanDeleteUsers = ActionOption.Unavailable,
            CanPinMessages = ActionOption.Enabled,
            CanSeeChannelMembers = ActionOption.Unavailable,
            CanInviteOtherUsers = ActionOption.Unavailable,
            CanEditChannelDescription = ActionOption.Unavailable,
            CanDeleteChat = ActionOption.Unavailable,
            Chat = this,
        };
    }

    public override ChatUser AddUser(MessengerUser user) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support adding users");

    public override ChatUser RemoveUser(MessengerUser user) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support removing users");

    public override void AddRole(Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support adding roles");

    public override void RemoveRole(Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support removing roles");

    public override void ChangeName(string name) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support renaming");

    public override void ChangeDescription(string description) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support description renaming");

    public override void ChangeUserRole(MessengerUser user, Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support changing user roles");
}