using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Room;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateRoomStatus(int id, UpdateRoomStatusDto UpdateRequest)
        {
            try
            {
                var updated = await _roomService.UpdateRoomStatusAsync(id, UpdateRequest.Status);
                if (!updated)
                    return NotFound(new { message = "Room not found" });

                return Ok(new { message = "Room status updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal Server Error",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetRoomStatus(int id)
        {
            try
            {
                var status = await _roomService.GetRoomStatusAsync(id);

                if (status == null)
                    return NotFound(new { message = "Room not found" });

                return Ok(new { status });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal Server Error",
                    error = ex.Message
                });
            }
        }

        [HttpGet("status-summary")]
        public async Task<IActionResult> GetStatusSummary()
        {
            try
            {
                var summary = await _roomService.GetRoomStatusSummaryAsync();
                return Ok(summary);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal Server Error",
                    error = ex.Message
                });
            }
        }
    }
}
