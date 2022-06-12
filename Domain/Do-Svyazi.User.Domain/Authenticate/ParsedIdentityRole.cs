using Microsoft.AspNetCore.Identity;

namespace Do_Svyazi.User.Domain.Authenticate;

public class ParsedIdentityRole : IdentityRole<Guid>
{
    public const string Admin = "Admin";
    public const string User = "User";

    public ParsedIdentityRole(string roleName)
        : base(roleName)
    {
    }

    protected ParsedIdentityRole() { }
}