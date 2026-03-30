using AutoMapper;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Mapping
{
    public class RoomMappingProfile : Profile
    {
        public RoomMappingProfile()
        {
            CreateMap<Room, RoomResponseDto>();
        }
    }
}
