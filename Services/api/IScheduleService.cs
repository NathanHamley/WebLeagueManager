using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;

namespace WebLeague.Services.api
{
    public interface IScheduleService
    {
        IList<Matchday> CreateSchedule(IList<Team> teams, bool withReverseFixtures = true);
    }
}
