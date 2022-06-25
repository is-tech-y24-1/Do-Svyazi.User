using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Roles;
using Do_Svyazi.User.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.DbContexts;

public interface IDbContext
{
    public DbSet<Chat> Chats { get; init; }
    public DbSet<MessengerUser> Users { get; init; }
    public DbSet<ChatUser> ChatUsers { get; init; }
    public DbSet<Role> Roles { get; init; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}