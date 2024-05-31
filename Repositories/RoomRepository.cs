using HotelBookingSystem.hotelContext;
using HotelBookingSystem.Interfaces;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.Repositories
{
    public class RoomRepository : Repository<Room> , iRoomRepository 
    {
        public RoomRepository(Context _context) : base(_context) 
        {
            
        }
    }
}
