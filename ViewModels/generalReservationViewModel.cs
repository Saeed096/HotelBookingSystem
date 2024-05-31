using System.ComponentModel;

namespace HotelBookingSystem.ViewModels
{
    public class generalReservationViewModel
    {
        public string name { get; set; }
        [DisplayName("National id")] 
        public string nationalId { get; set; }
        [DisplayName("Phone number")]
        public string phoneNumber { get; set; }
        [DisplayName("Branch name")] 
        public int branchId { get; set; }
        [DisplayName("Check in")] 
        public DateTime checkIn { get; set; }
        [DisplayName("Check out")]
        public DateTime checkOut { get; set; }
        [DisplayName("single rooms number")]
        public int singleRoomsNum { get; set; }
        [DisplayName("double rooms number")]
        public int doubleRoomsNum { get; set; }
        [DisplayName("suit rooms number")]
        public int suitRoomsNum { get; set; }
        public string? user { get; set; }
        public List<int> roomsCapacity { get; set; } 

    }
}
