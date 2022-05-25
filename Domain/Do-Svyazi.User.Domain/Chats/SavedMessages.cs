using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public class SavedMessages : Chat
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
    public string Name { get; init; }
    public string Description { get; init; }
    public List<ChatUser> Users { get; init; } = new ();
    public List<Role> Roles { get; init; } = new ();
}