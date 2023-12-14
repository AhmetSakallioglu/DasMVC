using dasMVC.Entities;
using Microsoft.AspNetCore.Mvc;

namespace dasMVC.Controllers
{
    public class UserController : Controller
    {
		private readonly DatabaseContext _databaseContext;

		public UserController(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		public IActionResult Index()
        {
            return View();
        }
    }
}
