using System.Linq;
using dasMVC.Entities;
using dasMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace dasMVC.Controllers
{
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
		public IActionResult Create(DepartmentModel departmentModel)
		{
			if (ModelState.IsValid)
			{
				Department department = new Department
				{
					DepartmentName = departmentModel.DepartmentName
				};

				_databaseContext.Departments.Add(department);
				_databaseContext.SaveChanges();

				return RedirectToAction(nameof(Index)); ;
			}

			return View();
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
