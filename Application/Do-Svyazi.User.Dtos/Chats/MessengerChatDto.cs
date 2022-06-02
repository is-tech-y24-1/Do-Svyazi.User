using Do_Svyazi.User.Dtos.Users;

namespace Do_Svyazi.User.Dtos.Chats;

public record MessengerChatDto
{
    public Guid Id { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public List<MessengerUserDto>? Users { get; init; } = new ();
}