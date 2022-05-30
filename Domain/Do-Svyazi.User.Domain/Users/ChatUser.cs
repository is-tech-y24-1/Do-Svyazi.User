using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Users;

public class ChatUser
{
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }

    public string NickName { get; init; }
    public Role Role { get; set; }

    public override bool Equals(object? obj) => Equals(obj as ChatUser);

    public override int GetHashCode() => HashCode.Combine(ChatId, UserId, NickName, Role);

    private bool Equals(ChatUser? chatUser) =>
        chatUser is not null &&
        UserId.Equals(chatUser.UserId) &&
        ChatId.Equals(chatUser.ChatId) &&
        NickName == chatUser.NickName &&
        Role.Equals(chatUser.Role);
}