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

    public Channel(MessengerUser user, string name, string description)
        : base(name, description)
    {
        var admin = new ChatUser
        {
            Role = _baseAdminRole,
            ChatId = Id,
            User = user,
        };

        Users.Add(admin);
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;
    }

    public override void AddUser(ChatUser chatUser)
    {
        if (chatUser is null)
            throw new ArgumentNullException(nameof(chatUser), "User to set is null");

        if (Users.Contains(chatUser))
            throw new Do_Svyazi_User_InnerLogicException($"User {chatUser.User.Name} already exists in chat {Name}");

        chatUser.Role = _baseUserRole;
        Users.Add(chatUser);
    }

    public override void RemoveUser(ChatUser chatUser)
    {
        if (chatUser is null)
            throw new ArgumentNullException(nameof(chatUser), "Role to set is null");

        if (!Users.Remove(chatUser))
            throw new Do_Svyazi_User_InnerLogicException($"User {chatUser.User.Name} doesn't exist in this chat {Name}");
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