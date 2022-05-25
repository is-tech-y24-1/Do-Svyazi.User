namespace Do_Svyazi.User.Dtos.Users;

public record MessengerUserDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string NickName { get; init; }
    public string Description { get; init; }
}