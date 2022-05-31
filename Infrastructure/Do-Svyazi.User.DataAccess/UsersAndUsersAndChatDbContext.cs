using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.DataAccess;

public class UsersAndUsersAndChatDbContext : DbContext, IUsersAndChatDbContext
{
    public UsersAndUsersAndChatDbContext(DbContextOptions<UsersAndUsersAndChatDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Chat> Chats { get; init; }
    public DbSet<MessengerUser> Users { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.Entity<Chat>()
            .HasMany(chat => chat.GetRoles);

        modelBuilder.Entity<ChatUser>()
            .HasOne(chatUser => chatUser.Chat)
            .WithMany(chat => chat.GetUsers);

        modelBuilder.Entity<Channel>();
        modelBuilder.Entity<GroupChat>();
        modelBuilder.Entity<PersonalChat>();
        modelBuilder.Entity<SavedMessages>();

        base.OnModelCreating(modelBuilder);
    }
}