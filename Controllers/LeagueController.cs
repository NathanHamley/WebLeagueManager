using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using WebLeague.Data;
using WebLeague.Models;

namespace WebLeague.Controllers
{
    public class LeagueController : Controller
    {
        private readonly ApplicationDbContext context;

        private readonly UserManager<ApplicationUser> userManager;

        private IIncludableQueryable<League, ApplicationUser> ContextWithUser => context.League.Include(league => league.User);

        public LeagueController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        // GET: League
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await getCurrentUser();
            List<League> list = await context.League.Where(league => league.User.Id == user.Id).ToListAsync();
            return View(list);
        }

        // GET: League/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await getCurrentUser();
            var league = await ContextWithUser
                .FirstOrDefaultAsync(filterByIdAndUser(id, user));
            if (league == null)
            {
                return NotFound();
            }
            return View(league);
        }
        // GET: League/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: League/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CreationDate")] League league)
        {
            var user = await getCurrentUser();
            if (ModelState.IsValid && user != null)
            {
                league.User = user;
                context.Add(league);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(league);
        }

        // GET: League/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await getCurrentUser();
            var league = await ContextWithUser.SingleOrDefaultAsync(filterByIdAndUser(id, user));
            if (league == null)
            {
                return NotFound();
            }
            return View(league);
        }

        // POST: League/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CreationDate")] League league)
        {
            if (id != league.Id)
            {
                return NotFound();
            }
            if (! await userOwnsLeague(id))
            {
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(league);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeagueExists(league.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(league);
        }

        // GET: League/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await getCurrentUser();
            var league = await ContextWithUser
                .FirstOrDefaultAsync(filterByIdAndUser(id, user));
            if (league == null)
            {
                return NotFound();
            }

            return View(league);
        }

        // POST: League/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await getCurrentUser();
            var league = await ContextWithUser.SingleOrDefaultAsync(filterByIdAndUser(id, user));
            context.League.Remove(league);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private static System.Linq.Expressions.Expression<Func<League, bool>> filterByIdAndUser(int? id, ApplicationUser user)
        {
            return league => league.Id == id && league.User.Id == user.Id;
        }

        private async Task<bool> userOwnsLeague(League league)
        {
            var user = await getCurrentUser();
            if (league == null || league.User == null || league.User.Id != user.Id)
            {
                return false;
            }
            return true;
        }

        private async Task<bool> userOwnsLeague(int leagueId)
        {
            var user = await getCurrentUser();
            var existing = await ContextWithUser.AsNoTracking()
                                                .SingleOrDefaultAsync(existing => existing.Id == leagueId);
            return existing == null || existing.User == null ? false : user.Id == existing.User.Id;
        }

        private async Task<ApplicationUser> getCurrentUser()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }

        private bool LeagueExists(int id)
        {
            return context.League.Any(e => e.Id == id);
        }
    }
}
