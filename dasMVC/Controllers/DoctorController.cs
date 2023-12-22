using dasMVC.Entities;
using dasMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dasMVC.Controllers
{
	[Authorize(Roles = "admin")]
	public class DoctorController : Controller
	{
		private readonly DatabaseContext _databaseContext;

		public DoctorController(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		public IActionResult Index()
		{
			List<DoctorViewModel> doctors = _databaseContext.Doctors
				.Include(d => d.Department)
				.Select(d => new DoctorViewModel
				{
					DoctorId = d.DoctorId,
					DoctorName = d.DoctorName,
					DoctorImage = d.DoctorImage,
					DepartmentId = d.DepartmentId,
					Department = new DepartmentModel
					{
						DepartmentId = d.Department.DepartmentId,
						DepartmentName = d.Department.DepartmentName
					}
				})
				.ToList();

			return View(doctors);
		}


        public IActionResult Create()
        {
            List<Department> departments = _databaseContext.Departments.ToList();

			ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDoctorModel doctorModel)
        {
            if (ModelState.IsValid)
            {
                if (_databaseContext.Doctors.Any(x => x.DoctorName.ToLower() == doctorModel.DoctorName.ToLower()
                && x.DepartmentId == doctorModel.DepartmentId))
                {
                    ModelState.AddModelError(nameof(doctorModel.DepartmentId), "This name corresponds to a doctor in this polyclinic.");
                    return View(doctorModel);
                }

                Doctor doctor = new Doctor
                {
                    DoctorName = doctorModel.DoctorName,
                    DepartmentId = doctorModel.DepartmentId
                };

                _databaseContext.Doctors.Add(doctor);
                _databaseContext.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Departments = new SelectList(_databaseContext.Departments.ToList(), "DepartmentId", "DepartmentName");

            return View(doctorModel);
        }

		public IActionResult Delete(int id)
		{
			Doctor doctor = _databaseContext.Doctors.Find(id);

			if (doctor != null)
			{
				_databaseContext.Doctors.Remove(doctor);
				_databaseContext.SaveChanges();
			}
			return RedirectToAction(nameof(Index));
		}

	}
}
