using Do_Svyazi.User.Application.DbContexts;
using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.DataAccess;

public class DoSvaziDbContext : DbContext, IDbContext
{
    public DoSvaziDbContext(DbContextOptions<DoSvaziDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Chat> Chats { get; init; }
    public DbSet<MessengerUser> Users { get; init; }
    public DbSet<ChatUser> ChatUsers { get; init; }
    public DbSet<Role> Roles { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);

        modelBuilder.Entity<Chat>()
            .HasOne(chatUser => chatUser.Creator);

        modelBuilder.Entity<ChatUser>()
            .HasOne(chatUser => chatUser.Chat)
            .WithMany(chat => chat.Users)
            .HasForeignKey(chatUser => chatUser.ChatId);

        modelBuilder.Entity<Chat>()
            .HasMany(chat => chat.Users)
            .WithOne(chatUser => chatUser.Chat);

        modelBuilder.Entity<Chat>()
            .HasMany(chat => chat.Roles)
            .WithOne(role => role.Chat);

        modelBuilder.Entity<Channel>().HasBaseType<Chat>();
        modelBuilder.Entity<GroupChat>().HasBaseType<Chat>();
        modelBuilder.Entity<PersonalChat>().HasBaseType<Chat>();
        modelBuilder.Entity<SavedMessages>().HasBaseType<Chat>();

        base.OnModelCreating(modelBuilder);
    }
}