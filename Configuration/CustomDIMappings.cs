using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Repositories.api;
using WebLeague.Repositories.impl;
using WebLeague.Services;
using WebLeague.Services.api;
using WebLeague.Services.impl;

namespace WebLeague.Configuration
{
    public class CustomDIMappings
    {

        public static void AddApplicationDIMappings(IServiceCollection services)
        {
            // Repositories
            services.AddScoped<ILeagueRepository, LeagueRepository>();
            services.AddScoped<ISeasonRepository, SeasonRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();
            services.AddScoped<IMatchdayRepository, MatchdayRepository>();
            services.AddScoped<IMatchRepository, MatchRepository>();


            //Services
            services.AddScoped<IScheduleService, ScheduleService>();
            services.AddScoped<IStandingsEvaluatorService, StandingsEvaluatorService>();
            services.AddScoped<ICascadingDeleteService, CascadingDeleteService>();
        }
    }
}
