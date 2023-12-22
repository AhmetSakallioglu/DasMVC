using System.Linq;
using dasMVC.Entities;
using dasMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dasMVC.Controllers
{
	[Authorize(Roles = "admin")]
	public class DepartmentController : Controller
    {
		private readonly DatabaseContext _databaseContext;

		public DepartmentController(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		public IActionResult Index()
        {
			List<DepartmentModel> departmentModels = _databaseContext.Departments
			.Select(x => new DepartmentModel { DepartmentId = x.DepartmentId, DepartmentName = x.DepartmentName })
			.ToList();

			return View(departmentModels);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(CreateDepartmentModel model)
		{
			if (ModelState.IsValid)
			{
				if (_databaseContext.Departments.Any(x => x.DepartmentName.ToLower() == model.DepartmentName.ToLower()))
				{
					ModelState.AddModelError(nameof(model.DepartmentName), "Username is already exists.");
					return View(model);
				}

				Department department = new Department
				{
					DepartmentName = model.DepartmentName
				};

				_databaseContext.Departments.Add(department);
				_databaseContext.SaveChanges();

				return RedirectToAction(nameof(Index));
			}

			return View(model);
		}


		public IActionResult Delete(int id)
		{
			Department department = _databaseContext.Departments.Find(id);

			if (department != null)
			{
				_databaseContext.Departments.Remove(department);
				_databaseContext.SaveChanges();
			}
			return RedirectToAction(nameof(Index));
		}

	}
}
