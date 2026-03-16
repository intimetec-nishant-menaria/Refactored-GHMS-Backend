using guest_house_management_backend.Data;
using guest_house_management_backend.Extensions;
using guest_house_management_backend.Middleware;
using guest_house_management_backend.Repositories.AvailableRoomRepo;
using guest_house_management_backend.Repositories.BookingRepo;
using guest_house_management_backend.Repositories.GuestRepo;
using guest_house_management_backend.Repositories.RoleRepo;
using guest_house_management_backend.Repositories.RoomRepo;
using guest_house_management_backend.Repositories.RoomTypeRepo;
using guest_house_management_backend.Repositories.UserRepo;
using guest_house_management_backend.Repositories.UserTokenRepo;
using guest_house_management_backend.Services.Auth;
using guest_house_management_backend.Services.AvailRoomService;
using guest_house_management_backend.Services.Bookings;
using guest_house_management_backend.Services.Email;
using guest_house_management_backend.Services.Guest;
using guest_house_management_backend.Services.Room;
using guest_house_management_backend.Services.RoomType;
using guest_house_management_backend.Services.UserManagement;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(builder.Configuration["Frontend:URL"]!)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
builder.Services.AddJwtService(builder.Configuration);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleRepository , RoleRepository>();
builder.Services.AddScoped<IUserManagementService , UserManagementService>();
builder.Services.AddTransient<IEmailSender , EmailSender>();
builder.Services.AddScoped<IUserTokenRepository , UserTokenRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IRoomTypeRepository, RoomTypeRepository>();
builder.Services.AddScoped<IRoomTypeService, RoomTypeService>();
builder.Services.AddScoped<IGuestRepository, GuestRepository>();
builder.Services.AddScoped<IGuestService, GuestService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IBookingCheckInOutService, BookingCheckInOutService>();
builder.Services.AddScoped<IAvailRoomRepository, AvailRoomRepostiory>();
builder.Services.AddScoped<IAvailRoomService, AvailRoomService>();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseGlobalExceptionMiddleware();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
