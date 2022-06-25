using Microsoft.AspNetCore.Identity;

namespace Do_Svyazi.User.Domain.Authenticate;

public class MessageIdentityRole : IdentityRole<Guid>
{
    public const string Admin = nameof(Admin);
    public const string ChatCreator = nameof(ChatCreator);
    public const string ChatAdmin = nameof(ChatAdmin);
    public const string User = nameof(User);

    public MessageIdentityRole(string roleName)
        : base(roleName)
    {
    }

    public MessageIdentityRole(string roleName, Guid chatId)
        : base(roleName)
    {
        ChatId = chatId;
    }

    protected MessageIdentityRole() { }

    public Guid ChatId { get; set; }
}