using Do_Svyazi.User.Dtos.Roles;

namespace Do_Svyazi.User.Dtos.Chats;

public record MessengerChatDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public IReadOnlyCollection<Guid> Users { get; init; } = new List<Guid>();
    public IReadOnlyCollection<RoleDto> Roles { get; init; } = new List<RoleDto>();
}