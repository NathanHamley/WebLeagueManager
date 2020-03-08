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

        public int createTeam(Team team)
        {
            context.Team.Add(team);
            return context.SaveChanges();
        }

        public int deleteTeam(int teamId)
        {
            var team = context.Team.Find(teamId);
            context.Team.Remove(team);
            return context.SaveChanges();
        }

        public void deleteMany(IEnumerable<Team> teams)
        {
            context.Team.RemoveRange(teams);
            context.SaveChanges();
        }
    }
}
