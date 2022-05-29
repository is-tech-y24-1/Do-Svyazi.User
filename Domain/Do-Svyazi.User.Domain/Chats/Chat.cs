using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;

namespace Do_Svyazi.User.Domain.Chats;

public abstract class Chat
{
    protected Chat() { }

    public Guid Id { get; protected init; } = Guid.NewGuid();
    public string Name { get; protected set; }
    public string Description { get; protected set; }

    // public long Tag { get; init; } ??
    public List<ChatUser> Users { get; init; } = new ();
    public List<Role> Roles { get; init; } = new ();

    public virtual void ChangeNameChat(string name)
    {
        Name = name ?? throw new NullReferenceException("Name chat is null");
    }

    public virtual void ChangeDescriptionChat(string description)
    {
        Description = description ?? throw new NullReferenceException("Name chat is null");
    }

    public List<ChatUser> GetChatUsersRole(Role role)
    {
        return Users.Where(user => user.Role.Name == role.Name).ToList();
    }

    public ChatUser GetChatUser(string nickName)
    {
        foreach (ChatUser user in Users.Where(user => user.NickName == nickName))
        {
            return user;
        }

        return new ChatUser();
    }

    public int GetChatUsersCount()
    {
        return Users.Count;
    }

    public virtual void AddChatUser(ChatUser chatUser)
    {
        if (chatUser is null)
            throw new NullReferenceException("ChatUser is null");

        Users.Add(chatUser);
    }

    public virtual void RemoveChatUser(ChatUser chatUser)
    {
        if (chatUser is null)
            throw new NullReferenceException("ChatUser is null");

        Users.Remove(chatUser);
    }

    public virtual void AddRole(Role role)
    {
        if (role is null)
            throw new NullReferenceException("Role is null");

        Roles.Add(role);
    }

    public virtual void RemoveRole(Role role)
    {
        if (role is null)
            throw new NullReferenceException("Role is null");

        Roles.Remove(role);
    }
}