using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class Channel : Chat
{
    private readonly Role _baseAdminRole;
    private readonly Role _baseUserRole;

    public Channel(MessengerUser creator, string name, string description)
        : base(name, description)
    {
        MaxUsersAmount = int.MaxValue;
        _baseAdminRole = new Role
        {
            Name = "admin",
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
            Chat = this,
        };

        _baseUserRole = new Role
        {
            Name = "base",
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
            Chat = this,
        };

        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;

        ChatUser user = CreateChatUser(creator, _baseAdminRole);
        Users.Add(user);
    }

    protected Channel()
    {
        _baseUserRole = new Role
        {
            Name = "base",
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
            Chat = this,
        };

        _baseAdminRole = new Role
        {
            Name = "admin",
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
            Chat = this,
        };
    }

    public override ChatUser AddUser(MessengerUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), $"User to add in chat {Name} is null");

        ChatUser newUser = CreateChatUser(user, _baseUserRole);

        if (IsUserExist(user))
            throw new Do_Svyazi_User_InnerLogicException($"User {user.Name} to add already exists in chat {Name}");

        Users.Add(newUser);

        return newUser;
    }

    public override ChatUser RemoveUser(MessengerUser user)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), $"User to remove from chat {Name} is null");

        ChatUser userToRemove = GetUser(user.Id);

        if (!IsUserExist(user))
            throw new Do_Svyazi_User_InnerLogicException($"User {user.Name} to remove doesn't exist in chat {Name}");

        Users.Remove(userToRemove);

        return userToRemove;
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
            throw new ArgumentNullException(nameof(role), $"Role to change in user {user.UserName} in chat {Name} is null");

        ChatUser chatUser = GetUser(user.UserName);

        chatUser.ChangeRole(role);
    }
}