using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dasMVC.Entities
{
	[Table("Doctors")]
	public class Doctor
	{
		[Key]
		public int DoctorId { get; set; }

		[Required]
		[StringLength(50)]
		public string DoctorName { get; set; }

		[StringLength(255)]
		public string? DoctorImage { get; set; } = "no-image.jpg";

		public int DepartmentId { get; set; }

		public Department Department { get; set; }
	}
}
