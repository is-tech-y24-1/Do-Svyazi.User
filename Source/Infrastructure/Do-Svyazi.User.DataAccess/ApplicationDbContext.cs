using Do_Svyazi.User.Domain.Authenticate;
using Do_Svyazi.User.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.DataAccess;

public class ApplicationDbContext : IdentityDbContext<MessengerUser, MessageIdentityRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}