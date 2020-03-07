using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebLeague.Models;
using WebLeague.Services;

namespace WebLeague.Controllers
{
    public class LeagueController : BaseController
    {

        private ILeagueRepository leagueRepository;

        public LeagueController(UserManager<ApplicationUser> userManager, ILeagueRepository leagueService) : base(userManager)
        {
            this.leagueRepository = leagueService;
        }

        // GET: League
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await getCurrentUser();
            var list = await leagueRepository.FindLeaguesForUser(user.Id);
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
            var league = await leagueRepository.FindLeagueForIdAndUser(id, user.Id);
            //var league = await ContextWithUser
            //    .FirstOrDefaultAsync(filterByIdAndUser(id, user));
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
                await leagueRepository.createLeague(league);
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
            var league = await leagueRepository.FindLeagueForIdAndUser(id, user.Id);
           
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
            var user = await getCurrentUser();
            if (! await leagueRepository.UserOwnsLeague(user.Id, id))
            {
                return Forbid();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    await leagueRepository.UpdateLeague(league);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!leagueRepository.LeagueExists(league.Id))
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
            var league = await leagueRepository.FindLeagueForIdAndUser(id, user.Id);
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
            if(await leagueRepository.UserOwnsLeague(user.Id, id))
            {
                await leagueRepository.DeleteLeague(id);
            }
            return RedirectToAction(nameof(Index));
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
    }
}
