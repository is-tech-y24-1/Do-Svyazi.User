namespace Do_Svyazi.User.Domain.Roles;

public class Role
{
    public Guid ChatId { get; init; }
    public string Name { get; set; }

    public ActionOption CanEditMessage { get; set; }
    public ActionOption CanDeleteMessage { get; set; }
    public ActionOption CanWriteMessage { get; set; }
    public ActionOption CanReadMessage { get; set; }

    public ActionOption CanAddUsers { get; set; }

    public ActionOption CanDeleteUsers { get; set; }

    public ActionOption CanPinMessages { get; set; }

    public ActionOption CanSeeChannelMembers { get; set; }

    public ActionOption CanInviteOtherUsers { get; set; }

    public ActionOption CanEditChannelDescription { get; set; }

    public ActionOption CanDeleteChat { get; set; }
}