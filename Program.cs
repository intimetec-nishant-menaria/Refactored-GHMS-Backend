using guest_house_management_backend.Data;
using guest_house_management_backend.Extensions;
using guest_house_management_backend.Hubs;
using guest_house_management_backend.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapperExtension();
builder.Services.AddSignalR();
builder.Services.AddApplicationServices();
builder.Services.AddApplicationRepository();

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
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionMiddleware();
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<BookingHub>("/api/hubs/bookings");
app.MapControllers();

app.Run();