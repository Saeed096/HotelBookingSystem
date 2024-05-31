using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingSystem.Models
{
    public class ApplicationUser : IdentityUser
    {

        public bool isOldClient { get; set; } = false;
        public List<Reservation>? reservations { get; set; }

       
    } 
}
