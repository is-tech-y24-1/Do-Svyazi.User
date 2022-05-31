using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Users;

public class ChatUser
{
    public ChatUser(MessengerUser user, Chat chat, Role role)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user), $"User to create chatUser in chat {chat?.Name} is null");

        if (chat is null)
            throw new ArgumentNullException(nameof(chat), $"Chat to create chatUser to {user.NickName} is null");

        if (role is null)
            throw new ArgumentNullException(nameof(role), $"Role to create chatUser to {user.NickName} is null");

        User = user;
        Chat = chat;
        Role = role;
    }

    protected ChatUser() { }

    public MessengerUser User { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();
    public Chat Chat { get; init; }
    public Role Role { get; private set; }

    public void ChangeRole(Role role)
    {
        if (role is null)
            throw new ArgumentNullException(nameof(role), $"Role to set to {User.NickName} in chat {Chat.Name} is null");

        Role = role;
    }

    public override bool Equals(object? obj) => Equals(obj as ChatUser);

    public override int GetHashCode() => HashCode.Combine(Chat, User);

    private bool Equals(ChatUser? chatUser) =>
        chatUser is not null &&
        User.Equals(chatUser.User) &&
        Chat.Equals(chatUser.Chat) &&
        Role.Equals(chatUser.Role);
}