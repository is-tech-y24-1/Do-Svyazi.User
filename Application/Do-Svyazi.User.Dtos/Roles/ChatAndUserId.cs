namespace Do_Svyazi.User.Dtos.Roles;

public record ChatAndUserId
{
    public Guid ChatId { get; init; }
    public Guid UserId { get; init; }
}