using Do_Svyazi.User.Domain.Chats;

namespace Do_Svyazi.User.Domain.Roles;

public class Role
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Chat Chat { get; init; }
    public string Name { get; set; }

    public ActionOption CanEditMessages { get; set; }
    public ActionOption CanDeleteMessages { get; set; }
    public ActionOption CanWriteMessages { get; set; }
    public ActionOption CanReadMessages { get; set; }
    public ActionOption CanAddUsers { get; set; }
    public ActionOption CanDeleteUsers { get; set; }
    public ActionOption CanPinMessages { get; set; }
    public ActionOption CanSeeChannelMembers { get; set; }
    public ActionOption CanInviteOtherUsers { get; set; }
    public ActionOption CanEditChannelDescription { get; set; }
    public ActionOption CanDeleteChat { get; set; }

    public override bool Equals(object? obj) => Equals(obj as Role);

    public override int GetHashCode() => HashCode.Combine(Chat, Name);

    private bool Equals(Role? role) =>
        role is not null &&
        Chat.Equals(role.Chat) &&
        Name.Equals(role.Name);
}