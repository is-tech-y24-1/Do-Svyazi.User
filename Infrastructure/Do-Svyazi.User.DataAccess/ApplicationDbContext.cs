using Do_Svyazi.User.Domain.Authenticate;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Do_Svyazi.User.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<MessageIdentityUser, MessageIdentityRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<MessageIdentityUser>()
                .Ignore(u => u.Email)
                .Ignore(u => u.PhoneNumber);
        }
    }
}