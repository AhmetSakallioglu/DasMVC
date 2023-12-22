using dasMVC.Entities;
using dasMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dasMVC.Controllers
{
	[Authorize(Roles = "admin")]
	public class AppointmentController : Controller
	{
		private readonly DatabaseContext _databaseContext;

		public AppointmentController(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		[AllowAnonymous]
		public IActionResult Index()
		{
			List<AppointmentViewModel> appointments = _databaseContext.Appointments
				.Include(d => d.Doctor)
				.Include(d => d.Department)
				.Select(d => new AppointmentViewModel
				{
					AppointmentId = d.AppointmentId,
					Date = d.Date,
					Time = d.Time,
					IsActive = d.IsActive,
					Doctor = new DoctorViewModel
					{
						DoctorId = d.Doctor.DoctorId,
						DoctorName = d.Doctor.DoctorName
					},
					Department = new DepartmentModel
					{
						DepartmentId = d.Department.DepartmentId,
						DepartmentName = d.Department.DepartmentName,
					}
				})
				.ToList();

			return View(appointments);
		}

		public IActionResult Create()
		{
			List<Department> departments = _databaseContext.Departments.ToList();
			List<Doctor> doctors = _databaseContext.Doctors.ToList();
			ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
			ViewBag.Departments = new SelectList(doctors, "DoctorId", "DoctorName");

			return View();
		}

		[HttpPost]
		public IActionResult Create(CreateAppointmentModel appointmentModel)
		{
			if (ModelState.IsValid)
			{
				if (_databaseContext.Appointments.Any(x => x.Date == appointmentModel.Date 
				&& x.Time == appointmentModel.Time && x.DoctorId == appointmentModel.DoctorId))
				{
					ModelState.AddModelError(nameof(appointmentModel.DoctorId), "This doctor is at another appointment at this time");
					return View(appointmentModel);
				}

				Appointment appointment = new Appointment
				{
					AppointmentId = appointmentModel.AppointmentId,
					Date = appointmentModel.Date,
					Time = appointmentModel.Time,
					IsActive = appointmentModel.IsActive,
					DoctorId = appointmentModel.DoctorId,
					DepartmentId = appointmentModel.DepartmentId
				};

				_databaseContext.Appointments.Add(appointment);
				_databaseContext.SaveChanges();

				return RedirectToAction(nameof(Index));
			}

			ViewBag.Departments = new SelectList(_databaseContext.Doctors.ToList(), "DoctorId", "DoctorName");
			ViewBag.Departments = new SelectList(_databaseContext.Departments.ToList(), "DepartmentId", "DepartmentName");

			return View(appointmentModel);
		}
	}
}
