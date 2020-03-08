using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;
using WebLeague.Services.Data;

namespace WebLeague.Services.api
{
    public interface IStandingsEvaluatorService
    {
        Task<StandingsDto> createStandings(int seasonId);
    }
}
