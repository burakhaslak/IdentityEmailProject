using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectEmailWithIdentity.Entities;

namespace ProjectEmailWithIdentity.Context
{
    public class EmailContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-IV25BQJ; initial catalog= ProjectEmailIdentityDb; integrated security=true");
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
