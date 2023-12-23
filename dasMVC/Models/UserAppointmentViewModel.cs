using dasMVC.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dasMVC.Models
{
    public class UserAppointmentViewModel
    {
        [Key]
        public int UserAppointmentId { get; set; }

        public Guid UserId { get; set; }

        public int AppointmentId { get; set; }
		public AppointmentViewModel appointment { get; set; }

		public int DoctorId { get; set; }
        public int DepartmentId { get; set; }
        public DoctorViewModel Doctor { get; set; }
        public DepartmentModel Department { get; set; }
    }
}
