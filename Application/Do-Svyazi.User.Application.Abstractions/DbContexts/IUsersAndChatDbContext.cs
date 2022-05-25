using Do_Svyazi.User.Domain.Chats;
using Do_Svyazi.User.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.Application.Abstractions.DbContexts;

public interface IUsersAndChatDbContext
{
    public DbSet<Chat> Chats { get; set; }
    public DbSet<MessengerUser> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}