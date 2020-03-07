using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;

namespace WebLeague.Repositories.api
{
    public interface ITeamRepository
    {
        Task createTeam(Team team);

        Task createTeams(IEnumerable<Team> teams);

        Task<int> deleteTeam(int teamId);
    }
}
