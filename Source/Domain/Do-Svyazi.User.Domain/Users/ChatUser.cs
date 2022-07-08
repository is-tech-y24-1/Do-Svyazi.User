using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Users;

public class ChatUser
{
    public ChatUser(MessengerUser user, Chat chat, Role role)
    {
        Chat = chat ?? throw new ArgumentNullException(nameof(chat), $"Chat to create chatUser with name {user.UserName} is null");
        Role = role ?? throw new ArgumentNullException(nameof(role), $"Role to create chatUser with userName {user.UserName} and {role} is null");
        ChatId = chat.Id;
        MessengerUserId = user.Id;
        ChatUserName = user.UserName;
    }

    public ChatUser() { }

    public Guid Id { get; init; } = Guid.NewGuid();
    public string ChatUserName { get; init; }
    public Guid MessengerUserId { get; init; }
    public Chat Chat { get; init; }
    public Guid ChatId { get; init; }
    public Role Role { get; private set; }

    public void ChangeRole(Role role) =>
        Role = role ?? throw new ArgumentNullException(nameof(role), $"Role to set in chat {Chat.Name} is null");

    public override bool Equals(object? obj) => Equals(obj as ChatUser);

    public override int GetHashCode() => HashCode.Combine(ChatId, MessengerUserId);

    private bool Equals(ChatUser? chatUser) =>
        chatUser is not null &&
        Chat.Id.Equals(chatUser.Chat.Id) &&
        MessengerUserId.Equals(chatUser.MessengerUserId);
}