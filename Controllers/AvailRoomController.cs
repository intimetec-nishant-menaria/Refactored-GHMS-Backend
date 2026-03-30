using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
using guest_house_management_backend.Services.AvailRoomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    [Authorize(Roles = "Admin,Ops,HR")]
    public class AvailRoomController : Controller
    {
        private readonly IAvailRoomService _roomService;
        public AvailRoomController(IAvailRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("availability")]
        public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAvailability([FromQuery] GenderEnum gender, [FromQuery] DateTime checkIn , [FromQuery] DateTime checkOut )
        {
            var result = await _roomService.GetAvailableRoomsAsync(gender , checkIn , checkOut);
            return Ok(result);
        }
    }
}
