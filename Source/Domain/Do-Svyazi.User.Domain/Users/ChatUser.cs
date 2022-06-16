using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Users;

public class ChatUser
{
    public ChatUser(MessengerUser user, Chat chat, Role role)
    {
        User = user ?? throw new ArgumentNullException(nameof(user), $"User to create chatUser in chat {chat.Name} is null");
        Chat = chat ?? throw new ArgumentNullException(nameof(chat), $"Chat to create chatUser to {user.NickName} is null");
        Role = role ?? throw new ArgumentNullException(nameof(role), $"Role to create chatUser to {user.NickName} is null");
        ChatId = chat.Id;
        MessengerUserId = user.Id;
    }

    public ChatUser() { }

    public MessengerUser User { get; init; }
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid MessengerUserId { get; init; }
    public Chat Chat { get; init; }
    public Guid ChatId { get; init; }
    public Role Role { get; private set; }

    public void ChangeRole(Role role) =>
        Role = role ?? throw new ArgumentNullException(nameof(role), $"Role to set to {User.NickName} in chat {Chat.Name} is null");

    public override bool Equals(object? obj) => Equals(obj as ChatUser);

    public override int GetHashCode() => HashCode.Combine(Chat, User);

    private bool Equals(ChatUser? chatUser) =>
        chatUser is not null &&
        User.Id.Equals(chatUser.User.Id) &&
        Chat.Id.Equals(chatUser.Chat.Id) &&
        MessengerUserId.Equals(chatUser.MessengerUserId);
}