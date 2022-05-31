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
        MaxUsersAmount = int.MaxValue;
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;

        ChatUser admin = CreateChatUser(messengerUser, BaseAdminRole);
        Users.Add(admin);
    }

    protected GroupChat() { }

    public override void AddUser(MessengerUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), $"User to add in chat {Name} is null");

        ChatUser newUser = CreateChatUser(user, _baseAdminRole);

        if (Users.Contains(newUser))
            throw new Do_Svyazi_User_InnerLogicException($"User {user.Name} to add already exists in chat {Name}");

        Users.Add(newUser);
    }

    public override void RemoveUser(MessengerUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), $"User to remove in chat {Name} is null");

        ChatUser removeUser = GetUser(user.NickName);

        if (!Users.Remove(removeUser))
            throw new Do_Svyazi_User_InnerLogicException($"User {user.Name} to remove doesn't exist in chat {Name}");
    }

    public override void AddRole(Role role)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role), $"Role to add in chat {Name} is null");

        if (Roles.Contains(role))
            throw new Do_Svyazi_User_InnerLogicException($"Role {role.Name} already exists in chat {Name}");

        Roles.Add(role);
    }

    public override void RemoveRole(Role role)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role), $"Role to remove in chat {Name} is null");

        if (!Roles.Remove(role))
            throw new Do_Svyazi_User_InnerLogicException($"Role {role.Name} to remove in chat {Name} doesn't exist");
    }

    public override void ChangeUserRole(MessengerUser user, Role role)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), $"User to change role in chat {Name} is null");

        if (role is null)
            throw new ArgumentNullException(nameof(role), $"Role to set to user {user.Name} in chat {Name} is null");

        ChatUser chatUser = GetUser(user.NickName);

        chatUser.ChangeRole(role);
    }
}