using Do_Svyazi.User.Dtos.Roles;
using Do_Svyazi.User.Dtos.Users;

namespace Do_Svyazi.User.Dtos.Chats;

public record MessengerChatDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public MessengerUserDto? Creator { get; init; }
    public IReadOnlyCollection<Guid> Users { get; init; } = new List<Guid>();
    public IReadOnlyCollection<RoleDto> Roles { get; init; } = new List<RoleDto>();
}