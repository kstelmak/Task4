using Microsoft.AspNetCore.Identity;
using Task4Attempt2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Task4Attempt2.Services
{
	public class MyServices
	{
		public void AddErrors(IdentityResult result, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ms)
		{
			foreach (IdentityError error in result.Errors)
				ms.AddModelError("", error.Description);
		}

		public async Task<bool> IsSignedInAsync(AppUser appUser, string pass, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ms, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
		{
			await signInManager.SignOutAsync();
			Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, pass, false, false);
			if (result.Succeeded)
			{
				appUser.LastLogin = DateTime.Now.ToString();
				IdentityResult updatingResult = await userManager.UpdateAsync(appUser);
				if (updatingResult.Succeeded)
					return true;
				else
					AddErrors(updatingResult,ms);
			}
			return false;
		}

		public async Task<bool> IsUserActiveAsync(AppUser currentUser, UserManager<AppUser> userManager)
		{

			if (currentUser.Status == "Blocked" || await userManager.FindByNameAsync(currentUser.UserName) == null)
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
