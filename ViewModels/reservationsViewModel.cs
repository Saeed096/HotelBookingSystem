using HotelBookingSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace HotelBookingSystem.ViewModels
{
    public class reservationsViewModel
    {
        public string name { get; set; }
        [DisplayName("National id")]
        public string nationalId { get; set; }
        [DisplayName("Phone number")]
        public string phoneNumber { get; set; }
        [DisplayName("Hotel branch id")]
        public int branchId { get; set; }

        [DisplayName("Hotel branch name")]
        public string branchName { get; set; }
        [ DisplayName("Client id")]
        public string clientId { get; set; }
       


        [DisplayName("Room id")]
        public int roomId { get; set; }

        [DisplayName("Checkin date")]
        public DateTime checkIn { get; set; }
        [DisplayName("Checkout date")]
        public DateTime checkout { get; set; }
        public decimal cost { get; set; }
    }
}
