using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using KSBookingMVC.Models;
namespace KSBookingMVC.Models
{
    public class CardDetails
    {
        [Key]
        public int CardsId { get; set; }
        [Required]
        [RegularExpression(@"^(19|20)\d\d-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")]
        public string? Date { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Name must be between 5 and 15 characters")]
        public string? NameOnCard { get; set; }

        [RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid card number")]
        public string Cardnumber { get; set; }

        [Required(ErrorMessage = "Month is required")]
        public string? Month { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(2023, 2026, ErrorMessage = "Year must be between 1900 and 2100")]
        public int year { get; set; }

        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid card number")]
        public string Cvv { get; set; }
        public int UsersId { get; set; }
        //[JsonIgnore]
        //[Required]
       // [ForeignKey("UsersId")]
        //public UserRegistration? UserRegistrations { get; set; }
    }
}
