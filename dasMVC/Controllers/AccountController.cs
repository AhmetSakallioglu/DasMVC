using dasMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace dasMVC.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
			if(ModelState.IsValid)
			{
				//Login İşlemleri..
			}

			return View();
		}

		public IActionResult Register()
		{
			return View();
		}

		public IActionResult Profile()
		{
			return View();
		}
	}
}
