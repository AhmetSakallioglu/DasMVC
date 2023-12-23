using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dasMVC.Entities
{
	public class UserAppointment
	{
		[Key]
		public int UserAppointmentId { get; set; }

		public Guid UserId { get; set; }

		public int AppointmentId { get; set; }

		public User User { get; set; }

		public Appointment Appointment { get; set; }

	}
}
