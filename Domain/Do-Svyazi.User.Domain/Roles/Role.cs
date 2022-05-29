namespace Do_Svyazi.User.Domain.Roles;

public class Role
{
    public Guid ChatId { get; init; }
    public string Name { get; set; }

    public ActionOption CanEdit { get; set; }
    public ActionOption CanDelete { get; set; }
    public ActionOption CanWrite { get; set; }
    public ActionOption CanRead { get; set; }
}