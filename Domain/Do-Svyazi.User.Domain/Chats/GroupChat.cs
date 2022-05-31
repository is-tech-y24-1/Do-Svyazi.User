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
        ChatUser admin = CreateChatUser(messengerUser, this, _baseAdminRole);
        Users.Add(admin);
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;
    }

    public override void AddUser(MessengerUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), "User to set is null");

        ChatUser newUser = CreateChatUser(user, this, _baseAdminRole);

        if (Users.Contains(newUser))
            throw new Do_Svyazi_User_InnerLogicException($"User {newUser.User.Name} already exists in chat {Name}");

        Users.Add(newUser);
    }

    public override void RemoveUser(MessengerUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), "User to set is null");

        ChatUser removeUser = GetUser(user.NickName);

        if (!Users.Remove(removeUser))
            throw new Do_Svyazi_User_InnerLogicException($"User {removeUser.User.Name} doesn't exist in chat {Name}");
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
            throw new Do_Svyazi_User_InnerLogicException($"Role {role.Name} doesn't exist in chat {Name}");
    }

    public override void ChangeUserRole(MessengerUser user, Role role)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), "User to set is null");
        if (role is null)
            throw new ArgumentNullException(nameof(role), "Role to set is null");

        ChatUser chatUser = GetUser(user.NickName);

        chatUser.Role = role;
    }
}