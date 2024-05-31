using HotelBookingSystem.hotelContext;
using HotelBookingSystem.Interfaces;
using HotelBookingSystem.Models;

namespace HotelBookingSystem.Repositories
{
    public class hotelBranchRepository : Repository<hotelBranch> , iHotelBranchRepository
    {
        public hotelBranchRepository(Context _context) : base(_context)
        {

        }
    }
}
