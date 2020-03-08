using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Data;
using WebLeague.Models;
using WebLeague.Repositories.api;

namespace WebLeague.Repositories.impl
{
    public class MatchRepository : IMatchRepository
    {

        private readonly ApplicationDbContext context;

        public MatchRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task deleteMany(IEnumerable<Match> matches)
        {
            context.RemoveRange(matches);
            return context.SaveChangesAsync();
        }

        public Task deleteMatch(Match match)
        {
            context.Match.Remove(match);
            return context.SaveChangesAsync();
        }

        public Task saveMatch(Match match)
        {
            if(match.Id <= 0)
            {
                context.Match.Add(match);
            }else
            {
                context.Match.Update(match);
            }
            return context.SaveChangesAsync();
        }

        public void updateResult(int matchId, int? homeScore, int? awayScore)
        {
            var match = context.Match.Where(m => m.Id == matchId).FirstOrDefault();
            match.HomeScore = homeScore;
            match.AwayScore = awayScore;
            context.Match.Update(match);
            context.SaveChanges();
        }
    }
}
