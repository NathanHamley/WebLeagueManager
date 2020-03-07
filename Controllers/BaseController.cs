using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebLeague.Models;

namespace WebLeague.Controllers
{

    
    public abstract class BaseController : Controller
    {

        private UserManager<ApplicationUser> userManager;

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        protected async Task<ApplicationUser> getCurrentUser()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }

        protected async Task<string> getCurrentUserId()
        {
            var user = await getCurrentUser();
            return user.Id;
        }
    }

  
}
