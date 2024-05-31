using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Interfaces
{
    public interface iUnitOfWork
    {
        public UserManager<ApplicationUser> userManager { get; }
        public SignInManager<ApplicationUser> signInManager { get; }
        public RoleManager<IdentityRole> roleManager { get; }
        public iRoomRepository roomRepository { get; }
        public iHotelBranchRepository hotelRepository { get; } 
        public iReservationRepository reservationRepository { get; }

        public int Save();

    }
}
