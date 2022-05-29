namespace Do_Svyazi.User.Domain.Roles;

public class Role
{
    public Guid ChatId { get; init; }
    public Guid UserId { get; init; }

    public string Name { get; set; }

    public ActionOption CanEdit { get; set; }
    public ActionOption CanDelete { get; set; }
    public ActionOption CanWrite { get; set; }
    public ActionOption CanRead { get; set; }

    public override bool Equals(object? obj) => Equals(obj as Role);

    public override int GetHashCode() =>
        HashCode.Combine(ChatId, UserId, Name, (int)CanEdit, (int)CanDelete, (int)CanWrite, (int)CanRead);

    public bool Equals(Role? role)
    {
        return role != null &&
               ChatId.Equals(role.ChatId) &&
               UserId.Equals(role.UserId) &&
               Name == role.Name &&
               CanEdit == role.CanEdit &&
               CanDelete == role.CanDelete &&
               CanWrite == role.CanWrite &&
               CanRead == role.CanRead;
    }
}