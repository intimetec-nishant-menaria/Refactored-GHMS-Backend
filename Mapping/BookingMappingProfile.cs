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
                .ForMember(des=>des.GuestName,
                    opt=>opt.MapFrom(src=>src.Guest.Name))
                .ForMember(des => des.GuestEmail,
                    opt => opt.MapFrom(src => src.Guest.Email))
                .ForMember(des => des.RoomNumber,
                    opt => opt.MapFrom(src => src.Room.RoomNumber));
        }
    }
}
