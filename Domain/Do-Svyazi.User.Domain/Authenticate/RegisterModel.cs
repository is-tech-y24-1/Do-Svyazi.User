using System.ComponentModel.DataAnnotations;

namespace Do_Svyazi.User.Domain.Authenticate;

public record RegisterModel
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; init; }
    [Required(ErrorMessage = "NickName is required")]
    public string NickName { get; init; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

}