using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;
using WebLeague.Services.api;
using WebLeague.Services.Data;

namespace WebLeague.Services.impl
{
    public class StandingsEvaluatorService : IStandingsEvaluatorService
    {
        private ISeasonRepository seasonRepository;

        public StandingsEvaluatorService(ISeasonRepository seasonRepository)
        {
            this.seasonRepository = seasonRepository;
        }

        public async Task<StandingsDto> createStandings(int seasonId)
        {
            var season = await seasonRepository.FindEverythingReadOnlyForSeason(seasonId);
            IList<Team> teams = season.Teams;
            IDictionary<int, StandingsTeam> standingTeams = new Dictionary<int, StandingsTeam>();
            initDictionary(teams, standingTeams);
            List<Match> allMatches = new List<Match>();
            season.Matchdays.ToList().ForEach(matchday => allMatches.AddRange(matchday.Matches));
            allMatches.ForEach(match => addScore(match, standingTeams));
            List<StandingsTeam> results = standingTeams.Values.ToList();
            results.Sort();
            return new StandingsDto(results);
        }

        private void addScore(Match match, IDictionary<int, StandingsTeam> standingTeams)
        {
            //If default team, skip!
            if(match.HomeTeam.Name == null || match.AwayTeam.Name == null)
            {
                return;
            }
            addSingleTeam(standingTeams[match.HomeTeam.Id], match.HomeScore, match.AwayScore);
            addSingleTeam(standingTeams[match.AwayTeam.Id], match.AwayScore, match.HomeScore);
        }

        private void addSingleTeam(StandingsTeam standingsTeam, int? goalsFor, int? goalsAgainst)
        {
            // This means match was not played, skip!
            if(goalsFor == null || goalsAgainst == null)
            {
                return;
            }
            standingsTeam.GoalsFor += (int)goalsFor;
            standingsTeam.GoalsAgainst += (int)goalsAgainst;
            standingsTeam.Played++;
            if(goalsFor > goalsAgainst)
            {
                standingsTeam.Wins++;
                standingsTeam.Points += 3;
            }else if(goalsFor < goalsAgainst)
            {
                standingsTeam.Losses++;
            }else
            {
                standingsTeam.Draws++;
                standingsTeam.Points++;
            }
        }

        private void initDictionary(IList<Team> teams, IDictionary<int, StandingsTeam> standingTeams)
        {
            foreach(Team team in teams) {
                standingTeams.Add(team.Id, new StandingsTeam(team.Name));
            }
        }
    }
}
