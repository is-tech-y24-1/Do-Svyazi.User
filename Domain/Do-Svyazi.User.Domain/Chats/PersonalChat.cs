using Do_Svyazi.User.Domain.Exceptions;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class PersonalChat : Chat
{
    private readonly int _maxUsersAmount = 2;

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

    public PersonalChat(MessengerUser firstMessengerUser, MessengerUser secondMessengerUser, string name, string description)
        : base(name, description)
    {
        MaxUsersAmount = _maxUsersAmount;
        BaseAdminRole = _baseAdminRole;
        BaseUserRole = _baseUserRole;

        ChatUser firstUser = CreateChatUser(firstMessengerUser, _baseAdminRole);
        ChatUser secondUser = CreateChatUser(secondMessengerUser, _baseAdminRole);

        Users.AddRange(new[] { firstUser, secondUser });
    }

    protected PersonalChat() { }

    public override void AddUser(MessengerUser user)
    {
        if (Users.Count >= MaxUsersAmount)
            throw new Do_Svyazi_User_BusinessLogicException($"User {user.Name} can't be added in chat {Name}");

        ChatUser newUser = CreateChatUser(user, _baseAdminRole);

        Users.Add(newUser);
    }

    public override void RemoveUser(MessengerUser user) =>
        throw new Do_Svyazi_User_BusinessLogicException($"Chat {Name} doesn't support removing users");

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