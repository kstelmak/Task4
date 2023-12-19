using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Task4Attempt2.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Task4Attempt2.Services;

namespace Task4Attempt2.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
		private MyServices services;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr, MyServices serv)
        {
            userManager = userMgr;
            signInManager = signinMgr;
			services = serv;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            Login login = new Login();
            return View(login);
        }

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(Login login)
		{
			if (ModelState.IsValid)
			{
				AppUser appUser = await userManager.FindByNameAsync(login.Name);
				if (appUser != null && appUser.Status != "Blocked")
				{
					if (await services.IsSignedInAsync(appUser, login.Password, ModelState, signInManager, userManager))
					{
						return RedirectToAction("Index", "Admin");
					}
				}

				ModelState.AddModelError(nameof(login.Name), "Login Failed: Invalid Email/password or you have been blocked");
			}
			return View(login);
		}

		public async Task<IActionResult> Logout()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}

		[AllowAnonymous]
		public ViewResult Create() => View();

        [AllowAnonymous]
        [HttpPost]
		public async Task<IActionResult> Create(User user)
		{
            if (ModelState.IsValid)
			{
				AppUser appUser = new AppUser {UserName = user.Name, Email = user.Email, FirstLogin = DateTime.Now.ToString(), LastLogin = DateTime.Now.ToString(), Status = "Active"};
				IdentityResult creationResult = await userManager.CreateAsync(appUser, user.Password);
				if (creationResult.Succeeded)
                {
					if (await services.IsSignedInAsync(appUser, user.Password, ModelState, signInManager, userManager))
					{
						return RedirectToAction("Index", "Admin");
					} 
                }                    
                else
				{
					services.AddErrors(creationResult, ModelState);
				}
			}
			return View(user);
		}		
	}
}
