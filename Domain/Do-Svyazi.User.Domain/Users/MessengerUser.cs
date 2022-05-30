using Do_Svyazi.User.Domain.Chats;

namespace Do_Svyazi.User.Domain.Users;

public class MessengerUser
{
    public Guid Id { get; protected init; } = Guid.NewGuid();
    public string Name { get; set; }
    public string NickName { get; set; }
    public string Description { get; set; }

    public List<Chat> Chats { get; } = new ();

    public override bool Equals(object? obj) => Equals(obj as MessengerUser);

    public override int GetHashCode() => HashCode.Combine(Id, Name, NickName, Description);

    private bool Equals(MessengerUser? messengerUser) =>
        messengerUser is not null &&
        Id.Equals(messengerUser.Id) &&
        Name.Equals(messengerUser.Name) &&
        NickName.Equals(messengerUser.NickName) &&
        Description.Equals(messengerUser.Description);
}