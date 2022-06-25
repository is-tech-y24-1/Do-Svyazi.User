using System.ComponentModel.DataAnnotations;

namespace Do_Svyazi.User.Domain.Authenticate;

public record LoginModel
{
    public string? UserName { get;  init; }
    public string? Email { get;  init; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; init; }

    public override int GetHashCode() => HashCode.Combine(UserName, Email);
}