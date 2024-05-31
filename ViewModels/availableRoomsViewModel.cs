using HotelBookingSystem.Models;

namespace HotelBookingSystem.ViewModels
{
    public class availableRoomsViewModel
    {
        public List<Room> rooms { get; set; }
        public bool isAvailable { get; set; } 
        public int availableNum { get; set; }
    }
}
