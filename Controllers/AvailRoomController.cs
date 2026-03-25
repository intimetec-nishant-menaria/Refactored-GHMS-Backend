using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.AvailRoomService;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class AvailRoomController : Controller
    {
        private readonly IAvailRoomService _roomService;
        public AvailRoomController(IAvailRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPost("availability")]
        public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAvailability(AvailabilityRequestDto request)
        {
            var result = await _roomService.GetAvailableRoomsAsync(request);
            return Ok(result);
        }
    }
}
