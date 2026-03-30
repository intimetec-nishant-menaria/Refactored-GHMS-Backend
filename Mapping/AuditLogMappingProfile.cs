using AutoMapper;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Models;

namespace guest_house_management_backend.Mapping
{
    public class AuditLogMappingProfile : Profile
    {
        public AuditLogMappingProfile()
        {
            CreateMap<AuditLog, AuditResponceDto>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.UserEmail,
                    opt => opt.MapFrom(src => src.User.Email));
        }
    }
}
