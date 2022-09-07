using System.ComponentModel.DataAnnotations;

namespace Do_Svyazi.User.Domain.Authenticate;

public class LoginModel
{
    public Guid Id { get; protected init; } = Guid.NewGuid();

    [Required(ErrorMessage = "NickName is required")]
    public string NickName { get;  set; }

    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }

    public override int GetHashCode() => HashCode.Combine(Id, NickName);
    public override bool Equals(object? obj) => Equals(obj as LoginModel);

    private bool Equals(LoginModel? login) =>
        login is not null &&
        Id.Equals(login.Id) &&
        NickName.Equals(login.NickName);
}