using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class GroupChat : Chat
{
    private readonly Role _baseAdminRole = new Role
    {
        CanEditMessages = ActionOption.Enabled,
        CanDeleteMessages = ActionOption.Enabled,
        CanWriteMessages = ActionOption.Enabled,
        CanReadMessages = ActionOption.Enabled,
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
        CanEditMessages = ActionOption.Disabled,
        CanDeleteMessages = ActionOption.Enabled,
        CanWriteMessages = ActionOption.Enabled,
        CanReadMessages = ActionOption.Enabled,
        CanAddUsers = ActionOption.Disabled,
        CanDeleteUsers = ActionOption.Disabled,
        CanPinMessages = ActionOption.Enabled,
        CanSeeChannelMembers = ActionOption.Enabled,
        CanInviteOtherUsers = ActionOption.Enabled,
        CanEditChannelDescription = ActionOption.Disabled,
        CanDeleteChat = ActionOption.Disabled,
    };

    public GroupChat(MessengerUser messengerUser, string name, string description)
        : base(name, description)
    {
        ChatUser admin = CreateUser(messengerUser, this, _baseAdminRole);
        Users.Add(admin);
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;
    }

    public override void AddUser(MessengerUser messengerUser)
    {
        if (messengerUser is null)
            throw new ArgumentNullException(nameof(messengerUser), "User to set is null");

        ChatUser user = CreateUser(messengerUser, this, _baseAdminRole);

        if (Users.Contains(user))
            throw new Do_Svyazi_User_InnerLogicException($"User {user.User.Name} already exists in chat {Name}");

        Users.Add(user);
    }

    public override void RemoveUser(MessengerUser messengerUser)
    {
        if (messengerUser is null)
            throw new ArgumentNullException(nameof(messengerUser), "User to set is null");

        ChatUser user = GetUser(messengerUser.NickName);

        if (!Users.Remove(user))
            throw new Do_Svyazi_User_InnerLogicException($"User {user.User.Name} doesn't exist in this chat {Name}");
    }

    public override void AddRole(Role role)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role), "User to set is null");

        if (Roles.Contains(role))
            throw new Do_Svyazi_User_InnerLogicException($"Role {role.Name} already exists in chat {Name}");

        Roles.Add(role);
    }

    public override void RemoveRole(Role role)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role), "Role to set is null");

        if (!Roles.Remove(role))
            throw new Do_Svyazi_User_InnerLogicException($"Role {role.Name} doesn't exist in this chat {Name}");
    }
}