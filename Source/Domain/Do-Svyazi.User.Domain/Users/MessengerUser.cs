using Do_Svyazi.User.Domain.Chats;
using Microsoft.AspNetCore.Identity;

namespace Do_Svyazi.User.Domain.Users;

public class MessengerUser : IdentityUser<Guid>
{
    private const string DefaultDescription = "No description";

    public MessengerUser(string name, string nickName, string? email, string? phoneNumber, string? description = null)
        : base(nickName)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "User name can't be null");
        if (string.IsNullOrWhiteSpace(nickName))
            throw new ArgumentNullException(nameof(nickName), "User nickName can't be null");

        Email = email;
        PhoneNumber = phoneNumber;
        Name = name;
        Description = string.IsNullOrEmpty(description) ? DefaultDescription : description;
        Id = Guid.NewGuid();
    }

    public MessengerUser() { }

    public string Name { get; private set; }
    public string Description { get; private set; }

    public virtual void ChangeNickName(string nickName)
    {
        if (string.IsNullOrWhiteSpace(nickName))
            throw new ArgumentNullException(nameof(nickName), "User nickName to change is null");

        UserName = nickName;
    }

    public virtual void ChangeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "User name to change is null");

        Name = name;
    }

    public virtual void ChangeDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException(nameof(description), "User description to change is null");

        Description = description;
    }

    public override bool Equals(object? obj) => Equals(obj as MessengerUser);

    public override int GetHashCode() => HashCode.Combine(Id, Name);

    private bool Equals(MessengerUser? messengerUser) =>
        messengerUser is not null &&
        Id.Equals(messengerUser.Id) &&
        Name.Equals(messengerUser.Name);
}