using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Data;
using WebLeague.Models;
using WebLeague.Services.api;

namespace WebLeague.Services.impl
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly ApplicationDbContext context;

        public SeasonRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task CreateSeason(Season season)
        {
            context.Add(season);
            return context.SaveChangesAsync();
        }

        public async Task<int> DeleteSeason(int seasonId)
        {
            var season = await context.Season.FindAsync(seasonId);
            context.Season.Remove(season);
            return await context.SaveChangesAsync();
        }

        public Task<Season> FindBySeasonIdAndLeagueId(int? seasonId, int? leagueId)
        {
            return context.Season.Include(season => season.Teams).Where(filterBySeasonAndLeague(seasonId, leagueId)).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<Season>> LoadSeasonForLeague(int? leagueId)
        {
            return await context.Season.Where(season => season.League.Id == leagueId).ToListAsync();
        }

        public bool SeasonExists(int seasonId)
        {
            return context.Season.Any(season => season.Id == seasonId);
        }

        public Task UpdateSeason(Season season)
        {
            context.Season.Update(season);
            return context.SaveChangesAsync();
        }


        private static System.Linq.Expressions.Expression<Func<Season, bool>> filterBySeasonAndLeague(int? seasonId, int? leagueId)
        {
            return season => season.Id == seasonId && season.League.Id == leagueId;
        }

        public  Task<Season> FindBySeasonIdAndLeagueIdWithSchedule(int? seasonId, int? leagueId)
        {
            return context.Season
                .Where(filterBySeasonAndLeague(seasonId, leagueId))

                .Include(season => season.Matchdays)
                    .ThenInclude(matchdays => matchdays.Matches)
                        .ThenInclude(matches => matches.HomeTeam).AsNoTracking()

                .Include(season => season.Matchdays)
                    .ThenInclude(matchdays => matchdays.Matches)
                        .ThenInclude(matches => matches.AwayTeam).AsNoTracking()

                .SingleOrDefaultAsync();
        }
    }
}
