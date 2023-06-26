using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace KSBookingMVC.Models
{
    public class BookingDetails
    {
        [Key]
        public int RoomsId { get; set; }
        public string? Hotelname { get; set; }
        public int Noofrooms { get; set; }
        public double Amount { get; set; }
        public int UsersId { get; set; }
        //JsonIgnore]
        //[ForeignKey("UsersId")]
        //[Required]
        // public UserRegistration? UserRegistrations { get; set; }
    }
}
