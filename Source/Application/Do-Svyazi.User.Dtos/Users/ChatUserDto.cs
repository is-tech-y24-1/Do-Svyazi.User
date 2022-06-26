using Do_Svyazi.User.Dtos.Roles;

namespace Do_Svyazi.User.Dtos.Users;

public record ChatUserDto
{
    public string ChatUserName { get; init; }
    public Guid MessengerUserId { get; init; }
    public RoleDto Role { get; init; }
}