using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Data;
using WebLeague.Models;

namespace WebLeague.Services.impl
{
    public class LeagueRepository : ILeagueRepository
    {

        private readonly ApplicationDbContext context;

        private IIncludableQueryable<League, ApplicationUser> ContextWithUser => context.League.Include(league => league.User);


        public LeagueRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<League>> FindLeaguesForUser(string userId)
        {
            return await context.League.Where(league => league.User.Id == userId).ToListAsync();
        }


        public bool LeagueExists(int id)
        {
            return context.League.Any(league => league.Id == id);
        }

        public Task<League> FindLeagueForIdAndUser(int? leagueId, string userId)
        {
            return ContextWithUser.SingleOrDefaultAsync(filterByIdAndUser(leagueId, userId));
        }

        public Task createLeague(League league)
        {
            context.Add(league);
            return context.SaveChangesAsync();
        }

        public Task UpdateLeague(League league)
        {
            context.League.Update(league);
            return context.SaveChangesAsync();
        }

        public async Task<int> DeleteLeague(int id)
        {
            var league = await context.League.FindAsync(id);
            context.League.Remove(league);
            return await context.SaveChangesAsync();
        }

        public Task<bool> UserOwnsLeague(string userId, int? leagueId)
        {
            return context.League.AnyAsync(filterByIdAndUser(leagueId, userId));
        }

        private static System.Linq.Expressions.Expression<Func<League, bool>> filterByIdAndUser(int? id, string userId)
        {
            return league => league.Id == id && league.User.Id == userId;
        }

    }
}
