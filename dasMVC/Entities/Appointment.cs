using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dasMVC.Entities
{
	[Table("Appointments")]
	public class Appointment
	{
		[Key]
		public int AppointmentId { get; set; }

		public DateTime Date { get; set; }

		public TimeSpan Time { get; set; }

		public bool IsActive { get; set; } = false;

		public int DoctorId { get; set; }

		public int DepartmentId { get; set; }

		public Doctor Doctor { get; set; }
		public Department Department { get; set; }
	}
}
