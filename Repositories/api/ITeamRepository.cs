﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;

namespace WebLeague.Repositories.api
{
    public interface ITeamRepository
    {
        int saveTeam(Team team);

        int deleteTeam(int teamId);

        void deleteMany(IEnumerable<Team> teams);
    }
}
