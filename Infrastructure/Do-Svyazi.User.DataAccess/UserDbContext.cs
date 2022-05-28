// using Do_Svyazi.User.Application.Abstractions.DbContexts;
// using Do_Svyazi.User.Domain.Roles;
// using Do_Svyazi.User.Domain.Users;
// using Microsoft.EntityFrameworkCore;
//
// namespace Do_Svyazi.User.DataAccess;
//
// public class UserDbContext : DbContext, IUserDbContext
// {
//     public DbSet<MessengerUser> Users { get; init; } = null!;
//
//     public UserDbContext(DbContextOptions<UserDbContext> options)
//         : base(options)
//     {
//         Database.EnsureCreated();
//     }
//
//     protected override void OnModelCreating(ModelBuilder modelBuilder)
//     {
//         ArgumentNullException.ThrowIfNull(modelBuilder);
//
//         modelBuilder.Entity<Role>().HasKey(role => new {role.ChatId, role.UserId});
//         modelBuilder.Entity<ChatUser>().HasKey(user => user.UserId);
//         
//         base.OnModelCreating(modelBuilder);
//     }
//
// }