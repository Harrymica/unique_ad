using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.ComponentModel.DataAnnotations;

namespace uniquead_App.Models
{
    [Table("profiles")]
    public class RegisterModel:BaseModel
    {
        [PrimaryKey("id", false)]
        public Guid Id { get; set; }

        [Column("name")]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }
      
        [Column("date")]
        [Required(ErrorMessage = "Date of birth is required.")]

        public DateTime Date { get; set; }

        [Column("phone_number")]
        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber {get; set; }

        [Column("district")]
        [Required(ErrorMessage = "District is required.")]
        public string District { get; set; }

        [Column("address")]
        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address can't exceed 200 characters.")]
        public string Address { get; set; }

        [Column("profession")]
        [Required(ErrorMessage = "Profession is required.")]
        public string Profession { get; set; }

        
        [Column("password")]
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string password { get; set; }

        [Column("email")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Column("userid")]
        public string userId { get; set; }

        [Column("role")]
        public string Role { get; set; }
    }
}
