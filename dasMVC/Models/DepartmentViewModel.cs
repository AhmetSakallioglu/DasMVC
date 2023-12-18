using System.ComponentModel.DataAnnotations;

namespace dasMVC.Models
{
	public class DepartmentModel
	{
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }
	}

	public class CreateDepartmentModel
	{
		[Required]
		[StringLength(50, ErrorMessage = "Department Name can be max 50 characters.")]
		public string DepartmentName { get; set; }
	}
}
