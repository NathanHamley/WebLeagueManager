using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using WebLeague.Data;

namespace WebLeague.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any movies.
                if (context.League.Any())
                {
                    return;   // DB has been seeded
                }

                context.League.AddRange(
                    new League
                    {
                        Name = "1. Bundesliga",
                        CreationDate = DateTime.Parse("1963-8-24"),
                    },

                    new League
                    {
                        Name = "2. Bundesliga",
                        CreationDate = DateTime.Parse("1974-8-2"),
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
