using AutoMapper;
using guest_house_management_backend.Mapping;

namespace guest_house_management_backend.Extensions
{
    public static class AutoMapperExtension
    {
        public static IServiceCollection AddAutoMapperExtension(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<RoomMappingProfile>();
                cfg.AddProfile<BookingMappingProfile>();
                cfg.AddProfile<GuestMappingProfile>();
                cfg.AddProfile<AuditLogMappingProfile>();
            });

            return services;
        }
    }
}
