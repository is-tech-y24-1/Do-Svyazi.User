using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Domain.Users;

public class ChatUser
{
    public Guid UserId { get; init; }
    public Guid ChatId { get; init; }

    public string NickName { get; init; }
    public Role Role { get; set; }
}