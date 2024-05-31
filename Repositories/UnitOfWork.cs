using HotelBookingSystem.hotelContext;
using HotelBookingSystem.Interfaces;
using HotelBookingSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelBookingSystem.Repositories
{
    public class UnitOfWork : iUnitOfWork, IDisposable
    {
        private readonly Context context;

        public UserManager<ApplicationUser> userManager { get; }
        public SignInManager<ApplicationUser> signInManager { get; }
        public RoleManager<IdentityRole> roleManager { get; }
        public iRoomRepository roomRepository { get; private set; }   
        public iHotelBranchRepository hotelRepository { get; private set; }
        public iReservationRepository reservationRepository { get; private set; }

        public UnitOfWork(UserManager<ApplicationUser> _userManager,
            SignInManager<ApplicationUser> _signInManager, RoleManager<IdentityRole> _roleManager, Context _context,
            iRoomRepository _roomRepository, iHotelBranchRepository hotelRepository, iReservationRepository reservationRepository)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            this.context = _context;
            roomRepository = _roomRepository;
            this.hotelRepository = hotelRepository;
            this.reservationRepository = reservationRepository;
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        void IDisposable.Dispose()
        {
            context.Dispose();
        }
    }
}
