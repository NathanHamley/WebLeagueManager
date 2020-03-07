using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
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
           // builder.Entity<IdentityUser>()
           //    .ToTable("Users").Property(p => p.Id).HasColumnName("User_Id");
            builder.Entity<ApplicationUser>()
                .ToTable("Users").Property(p => p.Id).HasColumnName("User_Id");

            /*builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "dbo");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "dbo");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "dbo");
            builder.Entity<IdentityRole>().ToTable("Roles", "dbo");*/
        }

        public DbSet<League> League { get; set; }
    }
}
