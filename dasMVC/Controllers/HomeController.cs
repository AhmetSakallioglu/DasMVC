using dasMVC.Models;
using dasMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace dasMVC.Controllers
{

	[Authorize]
	public class HomeController : Controller
	{
        private LanguageService _localization;

        public HomeController(LanguageService localization)
        {
            _localization = localization;
        }

        [AllowAnonymous]
		public IActionResult Index()
		{
			ViewBag.Home = _localization.Getkey("Welcome").Value;
			var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
			return View();
		}

		[AllowAnonymous]
		public IActionResult Privacy()
		{
			return View();
		}

		[AllowAnonymous]
		public IActionResult AccessDenied()
		{
			return View();
		}

		[AllowAnonymous]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}


        [AllowAnonymous]
        public IActionResult ChangeLanguage(string culture)
        {
			Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions()
				{
					Expires = DateTimeOffset.UtcNow.AddYears(1)
				});
            return Redirect(Request.Headers["Referer"].ToString ());
        }
    }
}
