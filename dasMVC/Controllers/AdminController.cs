using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dasMVC.Controllers
{
	[Authorize(Roles = "admin")]
	public class AdminController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
