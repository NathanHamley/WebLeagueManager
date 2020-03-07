using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Data;
using WebLeague.Models;
using WebLeague.Repositories.api;

namespace WebLeague.Repositories.impl
{
    public class TeamRepository : ITeamRepository
    {

        private readonly ApplicationDbContext context;

        public TeamRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task createTeam(Team team)
        {
            context.Team.Add(team);
            return context.SaveChangesAsync();
        }

        public Task createTeams(IEnumerable<Team> teams)
        {
            context.Team.AddRange(teams);
            return context.SaveChangesAsync();
        }

        public async Task<int> deleteTeam(int teamId)
        {
            var team = await context.Team.FindAsync(teamId);
            context.Team.Remove(team);
            return await context.SaveChangesAsync();
        }
    }
}
