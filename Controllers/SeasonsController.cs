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

        public SeasonsController(UserManager<ApplicationUser> userManager, ISeasonRepository seasonRepository,  
            ILeagueRepository leagueRepository,
            ITeamRepository teamRepository) : base(userManager)
        {
            this.seasonRepository = seasonRepository;
            this.leagueRepository = leagueRepository;
            this.teamRepository = teamRepository;
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

        private bool validateTeams(IList<Team> teams)
        {
            bool valid = true;
            foreach(var team in teams)
            {
                if(string.IsNullOrWhiteSpace(team.Name))
                {
                    valid = false;
                }
            }
            return valid;
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
            if (!await leagueRepository.UserOwnsLeague(await getCurrentUserId(), leagueId))
            {
                return Forbid();
            }
            var season = await seasonRepository.FindBySeasonIdAndLeagueId(pSeason.Id, leagueId);
            if(season == null)
            {
                return NotFound();
            }
            if (!string.IsNullOrWhiteSpace(teamName))
            {
                Team team = new Team();
                team.Name = teamName;
                await teamRepository.createTeam(team);
                ensureTeamAdded(season, team);
                await seasonRepository.UpdateSeason(season);
            }
            return RedirectToAction(nameof(Edit), new { Id = season.Id, leagueId });
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

            await teamRepository.deleteTeam(teamId);
            return RedirectToAction(nameof(Edit), new { Id = id, leagueId });
        }

        private void ensureTeamAdded(Season season, Team team)
        {
            if(season.Teams == null)
            {
                season.Teams = new List<Team>();
            }
            season.Teams.Add(team);
        }

        // GET: Seasons/Delete/5
        public async Task<IActionResult> Delete(int? id, int leagueId)
        {
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

            var season = await seasonRepository.FindBySeasonIdAndLeagueId(id, leagueId);
            await seasonRepository.DeleteSeason(id);
            return RedirectToAction(nameof(Index), new { leagueId });
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
