using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebLeague.Models;

namespace WebLeague.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>()
                .ToTable("Users").Property(p => p.Id).HasColumnName("User_Id");

        }

        public DbSet<League> League { get; set; }

        public DbSet<Season> Season { get; set; }

        public DbSet<Team> Team{ get; set; }

        public DbSet<Matchday> Matchday { get; set; }

        public DbSet<Match> Match { get; set; }
    }
}
