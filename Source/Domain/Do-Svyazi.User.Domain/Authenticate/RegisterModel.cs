using System.ComponentModel.DataAnnotations;

namespace Do_Svyazi.User.Domain.Authenticate;

public record RegisterModel
{
    [Required(ErrorMessage = "NickName is required")]
    public string NickName { get; init; }

    [Required(ErrorMessage = "Email is required")]
    public string Email { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; init; }

    public string PhoneNumber { get; init; }
}