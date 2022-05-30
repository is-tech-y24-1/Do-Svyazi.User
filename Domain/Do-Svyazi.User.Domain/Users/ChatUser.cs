using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Users;

public class ChatUser
{
    public MessengerUser User { get; init; }
    public Chat Chat { get; init; }
    public Role Role { get; set; }

    public override bool Equals(object? obj) => Equals(obj as ChatUser);

    public override int GetHashCode() => HashCode.Combine(Chat, User);

    private bool Equals(ChatUser? chatUser) =>
        chatUser is not null &&
        User.Equals(chatUser.User) &&
        Chat.Equals(chatUser.Chat) &&
        Role.Equals(chatUser.Role);
}