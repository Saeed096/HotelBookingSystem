using HotelBookingSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystem.Models
{
    public class Room
    {
        public Room()
        {
            pricePerDay = 1000 * (int)type;
            reservations = new List<Reservation>();
        }

        [Key]
        public int id { get; set; }

        public RoomType type { get; set; } = RoomType.Single;
        public decimal pricePerDay { get; set; }
        public DateOnly lastBookingPeriod;

        [ForeignKey("hotelBranch")]
        public int hotelId { get; set; }
        public hotelBranch hotelBranch { get; set; }

       
        public List<Reservation> reservations { get; set; }

       

    }
}
