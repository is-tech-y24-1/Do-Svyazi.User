using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Users;

public class ChatUser
{
    public MessengerUser User { get; init; }
    public Guid ChatId { get; init; }
    public Role Role { get; set; }

    public override bool Equals(object? obj) => Equals(obj as ChatUser);

    public override int GetHashCode() => HashCode.Combine(ChatId, User);

    private bool Equals(ChatUser? chatUser) =>
        chatUser is not null &&
        User.Equals(chatUser.User) &&
        ChatId.Equals(chatUser.ChatId) &&
        Role.Equals(chatUser.Role);
}