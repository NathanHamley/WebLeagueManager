using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebLeague.Services.Data
{
    public class StandingsTeam : IComparable<StandingsTeam>
    {
        public string Name { get; set; }
        public int Played { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int Points { get; set; }

        public StandingsTeam(string name)
        {
            Name = name;
        }

        public int CompareTo([AllowNull] StandingsTeam other)
        {
            if(other == null)
            {
                return -1;
            }
            //First by Points
            if(Points != other.Points)
            {
                return other.Points - Points;
            }
            //Then by goaldifference
            int myGoalDifference = GoalsFor - GoalsAgainst;
            int otherGoalDifference = other.GoalsFor - other.GoalsAgainst;
            if(myGoalDifference != otherGoalDifference)
            {
                return otherGoalDifference - myGoalDifference;
            }
            //Then by goals for
            if(GoalsFor != other.GoalsFor)
            {
                return other.GoalsFor - GoalsFor;
            }
            //Then by name
            if(Name != null)
            {
                return Name.CompareTo(other.Name);
            }else if(other.Name != null)
            {
                return other.Name.CompareTo(Name);
            }
            return 0;
            
        }
    }
}
