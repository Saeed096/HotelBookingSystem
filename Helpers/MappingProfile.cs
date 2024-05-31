using AutoMapper;
using HotelBookingSystem.Models;
using HotelBookingSystem.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Reservation, reservationsViewModel>(); 
        CreateMap<reservationsViewModel, Reservation>();
    }
}
