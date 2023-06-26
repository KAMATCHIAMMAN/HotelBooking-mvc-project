using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace KSBookingMVC.Models
{
    public class Register
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters")]
        public string? Name { get; set; }


        [EmailAddress(ErrorMessage = "Invalid email address")]

        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters")]
        public string? Password { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number")]
        public string? Phonenumber { get; set; }
    }
}
