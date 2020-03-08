using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLeague.Services.api
{
    // Current workaround for bad starting architecture decisions that will cleanup (by adding a service layer)
    // Helps do cascading deletes
    public interface ICascadingDeleteService
    {
        Task deleteEntireLeague(int id, string userId);

        Task deleteSeason(int seasonId, int leagueId);
        
    }
}
