using System.ComponentModel.DataAnnotations;

namespace uniquead_App.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Email or phone is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
    }

}
