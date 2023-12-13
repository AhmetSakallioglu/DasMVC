using dasMVC.Entities;
using dasMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace dasMVC.Controllers
{
	public class AccountController : Controller
	{
        private readonly DatabaseContext _databaseContext;
        public AccountController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

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

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
				{
					Username = model.Username,
					Password = model.Password
				};

				_databaseContext.Users.Add(user);
				int affectedRowCount = _databaseContext.SaveChanges();

				if(affectedRowCount == 0)
				{
					ModelState.AddModelError("", "User can not be added.");
				}
				else
				{
					return RedirectToAction(nameof(Login));
				}
            }

            return View();
        }

        public IActionResult Profile()
		{
			return View();
		}
	}
}
