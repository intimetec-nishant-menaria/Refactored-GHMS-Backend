using guest_house_management_backend.Repositories.AuditRepo;
using guest_house_management_backend.Repositories.AvailableRoomRepo;
using guest_house_management_backend.Repositories.BookingRepo;
using guest_house_management_backend.Repositories.RoleRepo;
using guest_house_management_backend.Repositories.RoomRepo;
using guest_house_management_backend.Repositories.UnitOfWorkRepo;
using guest_house_management_backend.Repositories.UserRepo;
using guest_house_management_backend.Repositories.UserTokenRepo;

namespace guest_house_management_backend.Extensions
{
    public static class ApplicationRespositoryExtension
    {
        public static IServiceCollection AddApplicationRepository(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IRoomRepository, RoomRepository>();
            //services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
            //services.AddScoped<IGuestRepository, GuestRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IAvailRoomRepository, AvailRoomRepostiory>();
            //services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuditRepository, AuditRepository>();

            return services;
        }
    }
}
