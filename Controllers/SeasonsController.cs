using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLeague.Models;
using WebLeague.Repositories.api;
using WebLeague.Services;
using WebLeague.Services.api;

namespace WebLeague.Controllers
{
    public class SeasonsController : BaseController
    {

        private readonly ILeagueRepository leagueRepository;
        private readonly ISeasonRepository seasonRepository;
        private readonly ITeamRepository teamRepository;
        private readonly IMatchdayRepository matchdayRepository;
        private readonly IMatchRepository matchRepository;

        //Start of better architecture, add services rather than using repositories
        //However wanted to get a quick first version for testing, learned that using repositories directly
        //is not smart

        private readonly IScheduleService scheduleService;
        private readonly IStandingsEvaluatorService standingsEvaluatorService;
        private readonly ICascadingDeleteService cascadingDeleteService;

        public SeasonsController(UserManager<ApplicationUser> userManager,
                                 ILeagueRepository leagueRepository,
                                 ISeasonRepository seasonRepository,
                                 ITeamRepository teamRepository,
                                 IMatchdayRepository matchdayRepository,
                                 IMatchRepository matchRepository,
                                 IScheduleService scheduleService,
                                 IStandingsEvaluatorService standingsEvaluatorService,
                                 ICascadingDeleteService cascadingDeleteService) : base(userManager)
        {
            this.leagueRepository = leagueRepository;
            this.seasonRepository = seasonRepository;
            this.teamRepository = teamRepository;
            this.matchdayRepository = matchdayRepository;
            this.matchRepository = matchRepository;
            this.scheduleService = scheduleService;
            this.standingsEvaluatorService = standingsEvaluatorService;
            this.cascadingDeleteService = cascadingDeleteService;
        }



        // GET: Seasons?{leagueId}
        public async Task<IActionResult> Index(int leagueId)
        {
            var seasons = await seasonRepository.LoadSeasonForLeague(leagueId);
            SetLeagueIdViewData(leagueId);
            await SetLeagueNameViewData(leagueId);
            return View(seasons);
        }

        // GET: Seasons/Details/5
        public async Task<IActionResult> Details(int? id, int leagueId)
        {
            SetLeagueIdViewData(leagueId);
            await SetLeagueNameViewData(leagueId);
            if (id == null)
            {
                return NotFound();
            }

            var season = await seasonRepository.FindBySeasonIdAndLeagueId(id, leagueId);
            if(season.Teams == null)
            {
                season.Teams = new List<Team>();
            }
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // GET: Seasons/Create
        public IActionResult Create(int leagueId)
        {
            SetLeagueIdViewData(leagueId);
            
            return View();
        }

        // POST: Seasons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int leagueId, [Bind("Id,Name")] Season season)
        {
            var league = await leagueRepository.FindLeagueForIdAndUser(leagueId, await getCurrentUserId());
            if (ModelState.IsValid && league != null)
            {
                season.League = league;
                await seasonRepository.CreateSeason(season);
                return RedirectToAction(nameof(Index), new { leagueId });
            }
            return View(season);
        }

        // GET: Seasons/Edit/5
        public async Task<IActionResult> Edit(int? id, int leagueId)
        {
            SetLeagueIdViewData(leagueId);
            if (id == null)
            {
                return NotFound();
            }

            var season = await seasonRepository.FindBySeasonIdAndLeagueId(id, leagueId);
            if (season == null)
            {
                return NotFound();
            }
            return View(season);
        }

        // POST: Seasons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int leagueId, [Bind("Id,Name,Status")] Season season)
        {
            if(! await leagueRepository.UserOwnsLeague(await getCurrentUserId(), leagueId))
            {
                return Forbid();
            }

            if (id != season.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await seasonRepository.UpdateSeason(season);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!seasonRepository.SeasonExists(season.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { leagueId });
            }
            return View(season);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTeam(int leagueId, [Bind("Id")] Season pSeason, string teamName)
        {
            var seasonId = pSeason.Id;
            if (!await leagueRepository.UserOwnsLeague(await getCurrentUserId(), leagueId))
            {
                return Forbid();
            }
            var season = await seasonRepository.FindBySeasonIdAndLeagueId(seasonId, leagueId);
            if(season == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(teamName))
            {
                Team team = new Team();
                team.Name = teamName;
                teamRepository.createTeam(team);
                ensureTeamAdded(season, team);
                await seasonRepository.UpdateSeason(season);
            }
            return RedirectToAction(nameof(Edit), new { Id = seasonId, leagueId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTeam(int id, int leagueId, int teamId)
        {
            if (!await leagueRepository.UserOwnsLeague(await getCurrentUserId(), leagueId))
            {
                return Forbid();
            }
            if (seasonRepository.FindBySeasonIdAndLeagueId(id, leagueId) == null)
            {
                return Forbid();
            }

            await cascadingDeleteService.deleteTeam(teamId);
            return RedirectToAction(nameof(Edit), new { Id = id, leagueId = leagueId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartSeason(int id, int leagueId)
        {
            Season season = await seasonRepository.FindBySeasonIdAndLeagueId(id, leagueId);
            var matchdays = scheduleService.CreateSchedule(season.Teams);
            await saveMatchdays(matchdays);
            season.Matchdays = matchdays;
            season.Status = SeasonStatus.Ongoing;
            await seasonRepository.UpdateSeason(season);
            return RedirectToAction(nameof(Index), new { leagueId });
        }

        // GET: Seasons/Delete/5
        public async Task<IActionResult> Delete(int? id, int leagueId)
        {
            SetLeagueIdViewData(leagueId);
            if (id == null)
            {
                return NotFound();
            }

            if (! await leagueRepository.UserOwnsLeague(await getCurrentUserId(), leagueId))
            {
                return Forbid();
            }

            var season = await seasonRepository.FindBySeasonIdAndLeagueId(id, leagueId);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // POST: Seasons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int leagueId)
        {

            if (!await leagueRepository.UserOwnsLeague(await getCurrentUserId(), leagueId))
            {
                return Forbid();
            }

            //var season = await seasonRepository.FindBySeasonIdAndLeagueIdWithSchedule(id, leagueId);
            await cascadingDeleteService.deleteSeason(id, leagueId);
            return RedirectToAction(nameof(Index), new { leagueId });
        }

        //private async Task fullDeleteSeason(Season season)
        //{
        //    List<Match> allMatches = new List<Match>();
        //    List<Matchday> allMatchDays = new List<Matchday>();
        //    foreach(var matchday in season.Matchdays)
        //    {
        //        allMatches.AddRange(matchday.Matches);
        //    }
        //    allMatchDays.AddRange(season.Matchdays);
        //    await matchRepository.deleteMany(allMatches);
        //    await matchdayRepository.deleteMany(allMatchDays);
        //    teamRepository.deleteMany(season.Teams);
        //    await seasonRepository.DeleteSeason(season.Id);
        //}

        public async Task<IActionResult> Standings(int? id, int leagueId)
        {
            SetLeagueIdViewData(leagueId);
            await SetLeagueNameViewData(leagueId);
            if (id == null)
            {
                return NotFound();
            }

            var season = await seasonRepository.FindBySeasonIdAndLeagueIdWithSchedule(id, leagueId);
            if (season == null || season.Status == SeasonStatus.Not_Started)
            {
                return NotFound();
            }
            await SetStandingsViewData(season);

            return View(season);
        }

        private async Task SetStandingsViewData(Season season)
        {
            var standingsDto = await standingsEvaluatorService.createStandings(season.Id); ;
            ViewBag.Standings = standingsDto.Standings;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStandings(int? id, int leagueId, Season model)
        {
            if (!await leagueRepository.UserOwnsLeague(await getCurrentUserId(), leagueId))
            {
                return Forbid();
            }

            if (id != model.Id)
            {
                return NotFound();
            }
            // Individually update results, as nothing else should be done here
            // Rethink vie/model/controller thing to add viewmodel so this is clean
            // Focus on getting it working for now
            saveResultsOnly(model.Matchdays);
            return RedirectToAction(nameof(Standings), new { id, leagueId });
        }

        private void saveResultsOnly(IList<Matchday> matchdays)
        {
            foreach (var matchday in matchdays)
            {
                foreach (var match in matchday.Matches)
                {

                    matchRepository.updateResult(match.Id, match.HomeScore, match.AwayScore);

                }
            }
        }

        private async Task saveMatchdays(IList<Matchday> matchdays)
        {
            foreach (var matchday in matchdays)
            {
                foreach (var match in matchday.Matches)
                {

                    await matchRepository.saveMatch(match);

                }
                await matchdayRepository.saveMatchday(matchday);
            }
        }

        private void ensureTeamAdded(Season season, Team team)
        {
            if (season.Teams == null)
            {
                season.Teams = new List<Team>();
            }
            season.Teams.Add(team);
        }
        private async Task SetLeagueNameViewData(int leagueId)
        {
            var userId = await getCurrentUserId();
            var league = await leagueRepository.FindLeagueForIdAndUser(leagueId, userId);
            ViewData["leagueName"] = league.Name;
        }

        private void SetLeagueIdViewData(int leagueId)
        {
            ViewData["leagueId"] = leagueId;
            ViewBag.LeagueId = leagueId;
        }
    }
}
