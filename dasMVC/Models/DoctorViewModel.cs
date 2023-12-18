using dasMVC.Entities;
using System.ComponentModel.DataAnnotations;


namespace dasMVC.Models
{
	public class DoctorViewModel
	{
		public int DoctorId { get; set; }
		public string DoctorName { get; set; }
		public string DoctorImage { get; set; } = "no-image.jpg";
		public int DepartmentId { get; set; }
		public DepartmentModel Department { get; set; }
	}

	public class CreateDoctorModel
	{
		[Required]
		[MaxLength(50, ErrorMessage = "Doctor Name can be max 50 characters.")]
		public string DoctorName { get; set; }

        public string DoctorImage { get; set; } = "no-image.jpg";

        [Required(ErrorMessage = "Please select a department.")]
		public int DepartmentId { get; set; }
	}

}