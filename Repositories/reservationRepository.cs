using HotelBookingSystem.hotelContext;
using HotelBookingSystem.Interfaces;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.Repositories
{
    public class reservationRepository : Repository<Reservation> , iReservationRepository
    {
        public reservationRepository(Context _context) : base(_context)
        {
            
        }

    }
}
