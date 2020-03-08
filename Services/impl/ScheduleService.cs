using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;
using WebLeague.Services.api;
using WebLeague.Common;

namespace WebLeague.Services.impl
{
    
    public class ScheduleService : IScheduleService
    {
        public IList<Matchday> CreateSchedule(IList<Team> teams, bool withReverseFixtures)
        {
            if(teams.Count % 2 != 0)
            {
                teams.Add(new Team());
            }
            teams.Shuffle();

            Team markerTeam = teams[1];

            List<Matchday> matchdays = new List<Matchday>();
            int matchDayNumber = 1;
            do
            {
                matchdays.Add(createMatchday(matchDayNumber, teams));
                advanceTeams(teams);
                matchDayNumber++;
            } while (teams[1] != markerTeam);

            if(withReverseFixtures)
            {
                matchdays.AddRange(createReverseMatchdays(matchdays));
            }
            return matchdays;
        }

        private IEnumerable<Matchday> createReverseMatchdays(IList<Matchday> matchdays)
        {
            IList<Matchday> reverseMatchdays = new List<Matchday>();
            int matchDayNumber = matchdays.Count +1;
            foreach(var matchday in matchdays)
            {
                reverseMatchdays.Add(reverseMatchday(matchDayNumber, matchday));
                matchDayNumber++;
            }
            return reverseMatchdays;
        }

        private Matchday reverseMatchday(int matchdayNumber, Matchday matchday)
        {
            IList<Match> reverseMatches = new List<Match>();
            foreach(Match firstLeg in matchday.Matches)
            {
                reverseMatches.Add(new Match(firstLeg.AwayTeam, firstLeg.HomeTeam, null, null));
            }
            return new Matchday(matchdayNumber, reverseMatches);
        }

        private void advanceTeams(IList<Team> teams)
        {
            if (teams.Count <= 2)
            {
                return;
            }
            teams.Insert(1, teams[teams.Count - 1]);
            teams.RemoveAt(teams.Count - 1);
        }

        private Matchday createMatchday(int number, IList<Team> teams)
        {
            Random random = new Random();
            IList<Match> matches = new List<Match>();
            for(int i=0; i<teams.Count / 2; i++)
            {
                //Randomize home and away
                if (random.NextDouble() > 0.5)
                {
                    //first team at Home
                    matches.Add(new Match(teams[i], teams[teams.Count - 1 - i], null, null));
                }
                else
                {
                    matches.Add(new Match(teams[teams.Count - 1 - i], teams[i], null, null));
                }
            }
            return new Matchday(number, matches);
        }
    }
}
