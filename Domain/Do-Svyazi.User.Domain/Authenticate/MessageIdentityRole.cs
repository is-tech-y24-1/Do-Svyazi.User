using Microsoft.AspNetCore.Identity;

namespace Do_Svyazi.User.Domain.Authenticate;

public class MessageIdentityRole : IdentityRole<Guid>
{
    public const string Admin = "Admin";
    public const string User = "User";

    public MessageIdentityRole(string roleName)
        : base(roleName)
    {
    }

    protected MessageIdentityRole() { }
}