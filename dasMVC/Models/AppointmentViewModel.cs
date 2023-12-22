using dasMVC.Entities;
using System.ComponentModel.DataAnnotations;

namespace dasMVC.Models
{
	public class AppointmentViewModel
	{
		public int AppointmentId { get; set; }
		public DateTime Date { get; set; }
		public TimeSpan Time { get; set; }
		public bool IsActive { get; set; } = false;
		public int DoctorId { get; set; }
		public int DepartmentId { get; set; }
		public DoctorViewModel Doctor { get; set; }
		public DepartmentModel Department { get; set; }
	}

	public class CreateAppointmentModel
	{
		[Required]
		public int AppointmentId { get; set; }

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public TimeSpan Time { get; set; }

		[Required]
		public bool IsActive { get; set; } = false;

		[Required(ErrorMessage = "Please select a doctor.")]
		public int DoctorId { get; set; }

		[Required(ErrorMessage = "Please select a department.")]
		public int DepartmentId { get; set; }
	}
}
