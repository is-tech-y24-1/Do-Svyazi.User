using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class PersonalChat : Chat
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
        CanSeeChannelMembers = ActionOption.Enabled,
        CanInviteOtherUsers = ActionOption.Unavailable,
        CanEditChannelDescription = ActionOption.Unavailable,
        CanDeleteChat = ActionOption.Enabled,
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
        CanSeeChannelMembers = ActionOption.Enabled,
        CanInviteOtherUsers = ActionOption.Unavailable,
        CanEditChannelDescription = ActionOption.Unavailable,
        CanDeleteChat = ActionOption.Enabled,
    };

    public PersonalChat(MessengerUser messengerUser, string name, string description)
        : base(name, description)
    {
        ChatUser admin = CreateUser(messengerUser, this, _baseAdminRole);
        Users.Add(admin);
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;
    }

    public override void AddUser(MessengerUser messengerUser)
    {
        if (Users.Count != 1)
            throw new Do_Svyazi_User_BusinessLogicException($"User {messengerUser.Name} can't added in chat {Name}");

        ChatUser user = CreateUser(messengerUser, this, _baseAdminRole);

        Users.Add(user);
    }

    public override void RemoveUser(MessengerUser messengerUser)
    {
        if (Users.Count != 2)
            throw new Do_Svyazi_User_BusinessLogicException($"Number users != 2 in chat {Name}");

        var user = GetUser(messengerUser.NickName);

        if (!Users.Remove(user))
            throw new Do_Svyazi_User_InnerLogicException($"Error removed user {user.User.Name} in chat {Name}");
    }

    public override void AddRole(Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Error added role in chat {Name}");

    public override void RemoveRole(Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Error deleted role in chat {Name}");

    public override void ChangeName(string name) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Error change name in chat {Name}");

    public override void ChangeDescription(string description) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Error change description in chat {Name}");
}