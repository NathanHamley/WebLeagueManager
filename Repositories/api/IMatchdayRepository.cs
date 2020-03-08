using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;

namespace WebLeague.Repositories.api
{
    public interface IMatchdayRepository
    {
        Task saveMatchday(Matchday matchday);

        Task deleteMatchday(Matchday matchday);

        Task deleteMany(IEnumerable<Matchday> matchdays);
    }
}
