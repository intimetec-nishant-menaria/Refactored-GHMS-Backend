using AutoMapper;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Mapping
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomResponseDto>()
                .ForMember(des=>des.RoomTypeName,
                        opt=>opt.MapFrom(src=>src.RoomType.RoomTypeName))
                .ForMember(des => des.Capacity,
                        opt => opt.MapFrom(src => src.RoomType.Capacity))
                .ForMember(des => des.PricePerNight,
                        opt => opt.MapFrom(src => src.RoomType.PricePerNight));
        }
    }
}
