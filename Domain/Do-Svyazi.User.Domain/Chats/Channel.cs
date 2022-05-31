using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class Channel : Chat
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
        CanInviteOtherUsers = ActionOption.Unavailable,
        CanEditChannelDescription = ActionOption.Enabled,
        CanDeleteChat = ActionOption.Enabled,
    };

    private readonly Role _baseUserRole = new Role
    {
        CanEditMessages = ActionOption.Disabled,
        CanDeleteMessages = ActionOption.Disabled,
        CanWriteMessages = ActionOption.Disabled,
        CanReadMessages = ActionOption.Enabled,
        CanAddUsers = ActionOption.Disabled,
        CanDeleteUsers = ActionOption.Disabled,
        CanPinMessages = ActionOption.Disabled,
        CanInviteOtherUsers = ActionOption.Enabled,
        CanEditChannelDescription = ActionOption.Disabled,
        CanDeleteChat = ActionOption.Disabled,
    };

    public Channel(MessengerUser messengerUser, string name, string description)
        : base(name, description)
    {
        MaxUsersAmount = int.MaxValue;
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;

        ChatUser admin = CreateChatUser(messengerUser, BaseAdminRole);

        Users.Add(admin);
    }

    protected Channel() { }

    public override void AddUser(MessengerUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), $"User to add in chat {Name} is null");

        ChatUser newUser = CreateChatUser(user, _baseAdminRole);

        if (Users.Contains(newUser))
            throw new Do_Svyazi_User_InnerLogicException($"User {newUser.User.Name} already exists in chat {Name}");

        Users.Add(newUser);
    }

    public override void RemoveUser(MessengerUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), $"User to remove from chat {Name} is null");

        ChatUser userToRemove = GetUser(user.NickName);

        if (!Users.Remove(userToRemove))
            throw new Do_Svyazi_User_InnerLogicException($"User {userToRemove.User.Name} to remove doesn't exist in chat {Name}");
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
            throw new ArgumentNullException(nameof(role), $"Role to remove from chat {Name} is null");

        if (!Roles.Remove(role))
            throw new Do_Svyazi_User_InnerLogicException($"Role {role.Name} doesn't exist in chat {Name}");
    }

    public override void ChangeUserRole(MessengerUser user, Role role)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), $"User to change role in chat {Name} is null");

        if (role is null)
            throw new ArgumentNullException(nameof(role), $"Role to change in user {user.NickName} in chat {Name} is null");

        ChatUser chatUser = GetUser(user.NickName);

        chatUser.ChangeRole(role);
    }
}