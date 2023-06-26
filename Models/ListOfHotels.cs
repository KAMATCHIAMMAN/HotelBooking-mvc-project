using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace KSBookingMVC.Models
{
    public class ListOfHotels
    {
        [Key]
        public int HotelsId { get; set; }

        [Required]
        public string? Imagelink { get; set; }
        [Required]
        public string? Hotelname { get; set; }
        [Required]
        public string? Place { get; set; }
        //[DataType(DataType.Date)]
        [Required]
        [RegularExpression(@"^(19|20)\d\d-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")]
        public string? Fromdate { get; set; }
        [Required]
        [RegularExpression(@"^(19|20)\d\d-(0[1-9]|1[0-2])-(0[1-9]|[12][0-9]|3[01])$")]
        public string? Todate { get; set; }
        [Required]
        public int Noofperson { get; set; }

    }
}
