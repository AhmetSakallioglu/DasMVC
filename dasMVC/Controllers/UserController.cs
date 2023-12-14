using AutoMapper;
using dasMVC.Entities;
using dasMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace dasMVC.Controllers
{
    public class UserController : Controller
    {
		private readonly DatabaseContext _databaseContext;
		private readonly IMapper _mapper;

		public UserController(DatabaseContext databaseContext, IMapper mapper)
		{
			_databaseContext = databaseContext;
			_mapper = mapper;
		}

		public IActionResult Index()
        {
			List<UserModel> users = _databaseContext.Users.ToList().Select(x => _mapper.Map<UserModel>(x)).ToList();

			//_databaseContext.Users.Select(x => new UserModel {Id = x.Id, FullName = x.FullName}).ToList();

            return View(users);
        }
    }
}
