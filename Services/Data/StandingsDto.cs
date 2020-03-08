using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLeague.Services.Data
{
    public class StandingsDto
    {
        public List<StandingsTeam> Standings { get; set; }

        public StandingsDto(List<StandingsTeam> standings)
        {
            Standings = standings;
        }
    }
}
