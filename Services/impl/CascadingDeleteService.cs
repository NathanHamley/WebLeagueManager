using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;
using WebLeague.Repositories.api;
using WebLeague.Services.api;

namespace WebLeague.Services.impl
{
    public class CascadingDeleteService : ICascadingDeleteService
    {
        private readonly ILeagueRepository leagueRepository;

        private readonly ISeasonRepository seasonRepository;

        private readonly IMatchRepository matchRepository;

        private readonly IMatchdayRepository matchdayRepository;

        private readonly ITeamRepository teamRepository;

        public CascadingDeleteService(ILeagueRepository leagueRepository, ISeasonRepository seasonRepository, IMatchRepository matchRepository, IMatchdayRepository matchdayRepository, ITeamRepository teamRepository)
        {
            this.leagueRepository = leagueRepository;
            this.seasonRepository = seasonRepository;
            this.matchRepository = matchRepository;
            this.matchdayRepository = matchdayRepository;
            this.teamRepository = teamRepository;
        }

        public async Task deleteEntireLeague(int id, string userId)
        {
            var league = await leagueRepository.FindLeagueForIdAndUser(id, userId);
            IList<Season> seasonsToRemove = new List<Season>();
            foreach(var season in league.Seasons)
            {
                await deleteSeasonRelevantInformation(season);
            }
            await seasonRepository.deleteMany(league.Seasons);
            await leagueRepository.DeleteLeague(id);
        }

        public async Task deleteSeason(int seasonId, int leagueId)
        {
            var season = await seasonRepository.FindBySeasonIdAndLeagueId(seasonId, leagueId);
            await deleteSeasonRelevantInformation(season);
            await seasonRepository.DeleteSeason(season.Id);
        }

        public async Task deleteSeasonRelevantInformation(Season season)
        {
            List<Match> allMatches = new List<Match>();
            List<Matchday> allMatchDays = new List<Matchday>();
            foreach (var matchday in season.Matchdays)
            {
                allMatches.AddRange(matchday.Matches);
            }
            allMatchDays.AddRange(season.Matchdays);
            await matchRepository.deleteMany(allMatches);
            await matchdayRepository.deleteMany(allMatchDays);
            teamRepository.deleteMany(season.Teams);
        }
    }
}
