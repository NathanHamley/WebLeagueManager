using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;

namespace WebLeague.Services.api
{
    public interface ISeasonRepository
    {

        Task<IEnumerable<Season>> LoadSeasonForLeague(int? leagueId);

        Task<Season> FindBySeasonIdAndLeagueId(int? seasonId, int? leagueId);

        Task<Season> FindBySeasonIdAndLeagueIdWithSchedule(int? seasonId, int? leagueId);

        Task CreateSeason(Season season);

        Task UpdateSeason(Season season);

        Task<int> DeleteSeason(int seasonId);

        bool SeasonExists(int seasonId);
    }
}
