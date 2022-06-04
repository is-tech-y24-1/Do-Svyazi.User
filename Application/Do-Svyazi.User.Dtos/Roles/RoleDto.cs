using Do_Svyazi.User.Domain.Roles;

namespace Do_Svyazi.User.Dtos.Roles;

public class RoleDto
{
    public string Name { get; init; }
    public ActionOption CanEditMessages { get; init; }
    public ActionOption CanDeleteMessages { get; init; }
    public ActionOption CanWriteMessages { get; init; }
    public ActionOption CanReadMessages { get; init; }
    public ActionOption CanAddUsers { get; init; }
    public ActionOption CanDeleteUsers { get; init; }
    public ActionOption CanPinMessages { get; init; }
    public ActionOption CanSeeChannelMembers { get; init; }
    public ActionOption CanInviteOtherUsers { get; init; }
    public ActionOption CanEditChannelDescription { get; init; }
    public ActionOption CanDeleteChat { get; init; }
}