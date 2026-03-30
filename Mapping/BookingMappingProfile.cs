using AutoMapper;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Mapping
{
    public class BookingMappingProfile : Profile
    {
        public BookingMappingProfile()  
        {
            CreateMap<Booking, BookingResponseDto>()
                .ForMember(des => des.RoomNumber,
                    opt => opt.MapFrom(src => src.Room.RoomNumber))
                .ForMember(des => des.Gender,
                    opt => opt.MapFrom(src => src.GuestGender));
        }
    }
}
