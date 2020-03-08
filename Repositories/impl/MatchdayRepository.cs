using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Data;
using WebLeague.Models;
using WebLeague.Repositories.api;

namespace WebLeague.Repositories.impl
{
    public class MatchdayRepository : IMatchdayRepository
    {

        private readonly ApplicationDbContext context;

        public MatchdayRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task deleteMany(IEnumerable<Matchday> matchdays)
        {
            context.Matchday.RemoveRange(matchdays);
            return context.SaveChangesAsync();
        }

        public Task deleteMatchday(Matchday matchday)
        {
            context.Matchday.Remove(matchday);
            return context.SaveChangesAsync();
        }

        public Task saveMatchday(Matchday matchday)
        {
            if(matchday.Id <= 0)
            {
                context.Matchday.Add(matchday);
            }else
            {
                context.Matchday.Update(matchday);
            }
            return context.SaveChangesAsync();
        }
    }
}
