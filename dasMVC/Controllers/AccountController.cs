using dasMVC.Entities;
using dasMVC.Models;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;

namespace dasMVC.Controllers
{
	public class AccountController : Controller
	{
        private readonly DatabaseContext _databaseContext;
		private readonly IConfiguration _configuration;
        public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
        {
            _databaseContext = databaseContext;
            _configuration = configuration;
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
				string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
				string saltedPassword = model.Password + md5Salt;
				string hashedPassword = saltedPassword.MD5();
				

                User user = new User()
				{
					Username = model.Username,
					Password = hashedPassword
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
