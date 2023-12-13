using System.ComponentModel.DataAnnotations;

namespace dasMVC.Models
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Username is required.")]
		[StringLength(30, ErrorMessage = "Username can be max 30 characters.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		[MinLength(6, ErrorMessage = "Password can be min 6 characters.")]
		[StringLength(16, ErrorMessage = "Password can be max 16 characters.")]
		public string Password { get; set; }
	}
}
