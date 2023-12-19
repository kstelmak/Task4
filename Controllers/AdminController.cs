using Task4Attempt2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Task4Attempt2.Services;

namespace Task4Attempt2.Controllers
{
	[Authorize]
	public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;        
		private MyServices services;

		public AdminController(UserManager<AppUser> usrMgr, MyServices serv)
        {
            userManager = usrMgr;
			services = serv;
		}
        public async Task<IActionResult> IndexAsync()
        {
            AppUser user = await userManager.GetUserAsync(HttpContext.User);
            ViewData["UserName"] = user.UserName;
            return View(userManager.Users);
        }  

		[HttpPost]
		public async Task<IActionResult> Delete(string[] userName)
		{
            foreach (string name in userName)
            {
				AppUser user = await userManager.FindByNameAsync(name);
				IdentityResult result = await userManager.DeleteAsync(user);
				if (!result.Succeeded)
                {
					services.AddErrors(result, ModelState);
                }				
			}
			if (await services.IsUserActiveAsync(await userManager.GetUserAsync(HttpContext.User), userManager))
			{
				return View("Index", userManager.Users);
			}
			return RedirectToAction("Login", "Account");					
		}

		[HttpPost]
		public async Task<IActionResult> ChangeStatus(string[] userName, string status)
		{
			foreach (string name in userName)
			{
				AppUser user = await userManager.FindByNameAsync(name);
				user.Status = status;
				IdentityResult blockingResult = await userManager.UpdateAsync(user);
				if (!blockingResult.Succeeded)
					services.AddErrors(blockingResult, ModelState);
			}            
            if (await services.IsUserActiveAsync(await userManager.GetUserAsync(HttpContext.User), userManager))
            {
                return RedirectToAction("Index");
			}
			return RedirectToAction("Login", "Account");
		}
	}
}
