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
        ChatUser admin = CreateChatUser(messengerUser, this, _baseAdminRole);
        Users.Add(admin);
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;
        MaxNumberUsers = 2;
    }

    public override void AddUser(MessengerUser user)
    {
        if (Users.Count != MaxNumberUsers - 1)
            throw new Do_Svyazi_User_BusinessLogicException($"User {user.Name} can't added in chat {Name}");

        ChatUser newUser = CreateChatUser(user, this, _baseAdminRole);

        Users.Add(newUser);
    }

    public override void RemoveUser(MessengerUser user)
    {
        if (Users.Count != MaxNumberUsers)
            throw new Do_Svyazi_User_BusinessLogicException($"Count users != {MaxNumberUsers} in chat {Name}");

        ChatUser removeUser = GetUser(user.NickName);

        if (!Users.Remove(removeUser))
            throw new Do_Svyazi_User_InnerLogicException($"User {removeUser.User.Name} doesn't exist in chat {Name}");
    }

    public override void AddRole(Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support adding roles");

    public override void RemoveRole(Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support removing roles");

    public override void ChangeName(string name) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support rename name");

    public override void ChangeDescription(string description) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support rename description");

    public override void ChangeUserRole(MessengerUser user, Role role) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support change user role");
}