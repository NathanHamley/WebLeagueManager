using System.Collections.Generic;
using System.Threading.Tasks;
using WebLeague.Models;

namespace WebLeague.Services
{
    public interface ILeagueRepository
    {

        Task<IEnumerable<League>> FindLeaguesForUser(string userId);

        Task<League> FindLeagueForIdAndUser(int? leagueId, string userId);

        Task createLeague(League league);

        Task UpdateLeague(League league);

        Task<int> DeleteLeague(int id);

        bool LeagueExists(int id);

        Task<bool> UserOwnsLeague(string userId, int? leagueId);
    }
}
