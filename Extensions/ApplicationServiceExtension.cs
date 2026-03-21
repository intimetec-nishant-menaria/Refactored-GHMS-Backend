using guest_house_management_backend.Services.Auth;
using guest_house_management_backend.Services.AvailRoomService;
using guest_house_management_backend.Services.Bookings;
using guest_house_management_backend.Services.Email;
using guest_house_management_backend.Services.Guest;
using guest_house_management_backend.Services.Room;
using guest_house_management_backend.Services.RoomType;
using guest_house_management_backend.Services.UserManagement;
using System.Runtime.CompilerServices;

namespace guest_house_management_backend.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoomTypeService, RoomTypeService>();
            services.AddScoped<IGuestService, GuestService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingCheckInOutService, BookingCheckInOutService>();
            services.AddScoped<IAvailRoomService, AvailRoomService>();
            services.AddTransient<IEmailSender, EmailSender>();

            return services;
        }
    }
}
