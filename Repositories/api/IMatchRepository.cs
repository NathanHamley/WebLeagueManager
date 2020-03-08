using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;

namespace WebLeague.Repositories.api
{
    public interface IMatchRepository
    {
        Task saveMatch(Match match);

        Task deleteMatch(Match match);

        Task deleteMany(IEnumerable<Match> matches);
    }
}
