using Do_Svyazi.User.Application.Abstractions.DbContexts;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Roles;
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

        modelBuilder.Entity<Role>().HasKey(role => new { role.ChatId, role.UserId });
        modelBuilder.Entity<ChatUser>().HasKey(user => user.UserId);

        modelBuilder.Entity<Channel>();
        modelBuilder.Entity<GroupChat>();
        modelBuilder.Entity<PersonalChat>();
        modelBuilder.Entity<SavedMessages>();

        base.OnModelCreating(modelBuilder);
    }
}