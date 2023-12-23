using dasMVC.Entities;
using dasMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
			ViewBag.Doctors = new SelectList(doctors, "DoctorId", "DoctorName");

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

				var doctor = _databaseContext.Doctors
					.Include(d => d.Department) 
					.FirstOrDefault(d => d.DoctorId == appointmentModel.DoctorId);

				if (doctor == null)
				{
					ModelState.AddModelError(nameof(appointmentModel.DoctorId), "Doctor not found");
					return View(appointmentModel);
				}

				int departmentId = doctor.DepartmentId;

				Appointment appointment = new Appointment
				{
					AppointmentId = appointmentModel.AppointmentId,
					Date = appointmentModel.Date,
					Time = appointmentModel.Time,
					IsActive = appointmentModel.IsActive,
					DoctorId = appointmentModel.DoctorId,
					DepartmentId = doctor.DepartmentId
				};

				_databaseContext.Appointments.Add(appointment);
				_databaseContext.SaveChanges();

				return RedirectToAction(nameof(Index));
			}

			ViewBag.Doctors = new SelectList(_databaseContext.Doctors.ToList(), "DoctorId", "DoctorName");
			ViewBag.Departments = new SelectList(_databaseContext.Departments.ToList(), "DepartmentId", "DepartmentName");

			return View(appointmentModel);
		}

		public IActionResult Delete(int id)
		{
			Appointment appointment = _databaseContext.Appointments.Find(id);

			if (appointment != null)
			{
				_databaseContext.Appointments.Remove(appointment);
				_databaseContext.SaveChanges();
			}
			return RedirectToAction(nameof(Index));
		}

		private Guid GetCurrentUserId()
		{
			// Eğer kullanıcı kimliğini HttpContext.User'dan almak istiyorsanız:
			var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

			// Eğer kullanıcı kimliği varsa ve değeri geçerli bir Guid ise döndür, yoksa default(Guid) veya başka bir değer döndür.
			return userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var userId) ? userId : default(Guid);
		}



		public IActionResult RandevuOlustur(int id)
		{
			if (ModelState.IsValid)
			{
				Guid userId = GetCurrentUserId();

				UserAppointment userAppointment = new UserAppointment
				{
					UserId = userId,
					AppointmentId = id
				};

				_databaseContext.UserAppointments.Add(userAppointment);
				_databaseContext.SaveChanges();

				// Randevu başarıyla oluşturulduktan sonra, Appointment'ın IsActive özelliğini güncelle
				var appointmentToUpdate = _databaseContext.Appointments.Find(id);

				if (appointmentToUpdate != null)
				{
					// Güncelleme işlemi
					appointmentToUpdate.IsActive = true;
					_databaseContext.SaveChanges();
				}

				return RedirectToAction(nameof(Index));
			}

			return View();
		}


			[AllowAnonymous]
        public IActionResult DepartmentListPartial()
        {
            List<DepartmentModel> departmentModels = _databaseContext.Departments
                .Select(x => new DepartmentModel { DepartmentId = x.DepartmentId, DepartmentName = x.DepartmentName })
                .ToList();

            return PartialView("_DepartmentListPartial", departmentModels);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetDoctors(int departmentId)
		{
			Console.WriteLine($"GetDoctors Action Called with DepartmentId: {departmentId}");

			List<DoctorViewModel> doctors = _databaseContext.Doctors
                .Where(d => d.DepartmentId == departmentId)
                .Select(d => new DoctorViewModel
                {
                    DoctorId = d.DoctorId,
                    DoctorName = d.DoctorName,
                    // Include other properties as needed
                })
                .ToList();

            return Json(doctors);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetDates(int doctorId)
        {
            // Bu bölümü kendi veritabanı sorgularınıza ve iş mantığınıza göre özelleştirin.
            List<DateTime> dates = _databaseContext.Appointments
                .Where(a => a.DoctorId == doctorId && a.Date > DateTime.Now)
                .OrderBy(a => a.Date)
                .Select(a => a.Date)
                .Distinct()
                .ToList();

            return Json(dates);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult GetTimes(DateTime date, int doctorId)
        {
            // Bu bölümü kendi veritabanı sorgularınıza ve iş mantığınıza göre özelleştirin.
            List<TimeSpan> times = _databaseContext.Appointments
                .Where(a => a.DoctorId == doctorId && a.Date == date && a.Date > DateTime.Now)
                .OrderBy(a => a.Time)
                .Select(a => a.Time)
                .ToList();

            return Json(times);
        }


        [AllowAnonymous]
        public IActionResult Appointments()
        {

            List<AppointmentViewModel> appointments = _databaseContext.Appointments
                .Include(d => d.Doctor)
                .Include(d => d.Department)
				.Where(d => !d.IsActive)
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

		[AllowAnonymous]
		public IActionResult MyAppointments()
		{
			List<UserAppointmentViewModel> userAppointments = _databaseContext.UserAppointments
				.Include(ua => ua.Appointment)
					.ThenInclude(a => a.Doctor)
				.Include(ua => ua.Appointment.Department) 
				.Select(ua => new UserAppointmentViewModel
				{
					UserAppointmentId = ua.UserAppointmentId,
					UserId = ua.UserId,
					AppointmentId = ua.AppointmentId,
					appointment = new AppointmentViewModel
					{
						AppointmentId = ua.Appointment.AppointmentId,
						Date = ua.Appointment.Date,
						Time = ua.Appointment.Time,
						Doctor = new DoctorViewModel
						{
							DoctorId = ua.Appointment.Doctor.DoctorId,
							DoctorName = ua.Appointment.Doctor.DoctorName
						},
						Department = new DepartmentModel
						{
							DepartmentId = ua.Appointment.Department.DepartmentId,
							DepartmentName = ua.Appointment.Department.DepartmentName
						}
					}
				})
				.ToList();

			return View(userAppointments);
		}




	}
}
