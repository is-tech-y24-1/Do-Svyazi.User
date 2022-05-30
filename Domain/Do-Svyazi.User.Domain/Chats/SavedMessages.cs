using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class SavedMessages : Chat
{
    private readonly Role _baseAdminRole = new Role
    {
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
    };

    private readonly Role _baseUserRole = new Role
    {
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
    };

    public SavedMessages(MessengerUser user, string name, string description)
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

    public override void AddUser(ChatUser chatUser) =>
        throw new Do_Svyazi_User_BusinessLogicException($"User {chatUser.User.Name} can't added in chat {Name}");

    public override void RemoveUser(ChatUser chatUser) =>
        throw new Do_Svyazi_User_BusinessLogicException($"User {chatUser.User.Name} can't removed in chat {Name}");

    public override void AddRole(Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Error added role in chat {Name}");

    public override void RemoveRole(Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Error deleted role in chat {Name}");

    public override void ChangeName(string name) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Error change name in chat {Name}");

    public override void ChangeDescription(string description) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Error change description in chat {Name}");
}