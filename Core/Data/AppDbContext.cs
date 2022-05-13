using Bitly.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bitly.Core.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Url> Urls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Url>().HasIndex(x => x.Key).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}

