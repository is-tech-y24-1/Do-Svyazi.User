using Do_Svyazi.User.Domain.Chats;

namespace Do_Svyazi.User.Domain.Users;

public class MessengerUser
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
    public string Name { get; set; }
    public string NickName { get; set; }
    public string Description { get; set; }

    public List<Chat> Chats { get; set; } = new ();
}