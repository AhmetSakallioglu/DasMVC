using das.Models;
using dasMVC.Entities;
using dasMVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.Encrypt.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace dasMVC.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{
		private readonly DatabaseContext _databaseContext;
		private readonly IConfiguration _configuration;

		public AccountController(DatabaseContext databaseContext, IConfiguration configuration)
		{
			_databaseContext = databaseContext;
			_configuration = configuration;
		}
		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}
		[AllowAnonymous]
		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				string hashedPassword = DoMD5HashedString(model.Password);

				User user = _databaseContext.Users.SingleOrDefault(x => x.Username.ToLower() == model.Username.ToLower() && x.Password == hashedPassword);

				if (user != null)
				{
					if (user.Locked)
					{
						ModelState.AddModelError(nameof(model.Username), "User is locked");
						return View(model);
					}

					List<Claim> claims = new List<Claim>();
					claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
					claims.Add(new Claim(ClaimTypes.Name, user.FullName ?? string.Empty));
					claims.Add(new Claim(ClaimTypes.Role, user.Role));
					claims.Add(new Claim("Username", user.Username));

					ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					ClaimsPrincipal principal = new ClaimsPrincipal(identity);

					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Username is password is incorrect.");
				}
			}
			return View(model);
		}
		[AllowAnonymous]
		public IActionResult Register()
		{
			return View();
		}
		[AllowAnonymous]
		[HttpPost]
		public IActionResult Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{

				if (_databaseContext.Users.Any(x => x.Username.ToLower() == model.Username.ToLower()))
				{
					ModelState.AddModelError(nameof(model.Username), "Username is already exists.");
					View(model);
				}

				string hashedPassword = DoMD5HashedString(model.Password);

				User user = new()
				{
					Username = model.Username,
					Password = hashedPassword
				};

				_databaseContext.Users.Add(user);
				int affectedRowCount = _databaseContext.SaveChanges();

				if (affectedRowCount == 0)
				{
					ModelState.AddModelError("", "User can not be added.");
				}
				else
				{
					return RedirectToAction(nameof(Login));
				}
			}
			return View(model);
		}

		private string DoMD5HashedString(string s)
		{
			string md5Salt = _configuration.GetValue<string>("AppSettings:MD5Salt");
			string salted = s + md5Salt;
			string hashed = salted.MD5();
			return hashed;
		}

		public IActionResult Profile()
		{
			return View();
		}


        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
