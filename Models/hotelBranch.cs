using System.ComponentModel.DataAnnotations;

namespace HotelBookingSystem.Models
{
    public class hotelBranch
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public List<Reservation>? reservations { get; set; }
    }
}
