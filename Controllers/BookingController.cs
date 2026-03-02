using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.BookingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("available")]
        public async Task<IActionResult> GetAllAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest)
        {
            var res = await _bookingService.GetAllAvailableRooms(roomAvaiblityRequest);
            Console.WriteLine(res);
            return Ok(res);
        }
    }
}
