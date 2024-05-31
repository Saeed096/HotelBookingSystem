using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystem.Models
{
    public class Reservation
    {
        //[Key] 
        //public int id { get; set; }
        public string name { get; set; }
        [DisplayName("National id")] 
        public string nationalId { get; set; }
        [DisplayName("Phone number")]
        public string phoneNumber { get; set; }

        [DisplayName("Hotel branch id"), ForeignKey("hotelBranch")]
        public int branchId { get; set; }

        public hotelBranch hotelBranch { get; set; }


        [ForeignKey("client"), DisplayName("Client id")]
        public string clientId { get; set; }
        public ApplicationUser client { get; set; }



        [ForeignKey("room"), DisplayName("Room id")]
        public int roomId { get; set; }
        public Room room { get; set; }

        [DisplayName("Checkin date")]
        public DateTime checkIn { get; set; }
        [DisplayName("Checkout date")]
        public DateTime checkout { get; set; }

        public decimal cost { get; set; }

        public int adultsNum { get; set; } 
        public int childrenNum { get; set; }
    }
}
