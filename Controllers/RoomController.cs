using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [ApiController]
    [Route("api/Room")]
    [Authorize(Roles = "Admin,Ops")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("summary")]
        public async Task<ActionResult<RoomsSummaryDto>> GetStatusSummary()
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAllRoomsAsync([FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] int roomStatus, [FromQuery] string? roomNumber)
        {
            try
            {
                var rooms = await _roomService.getAllRoomAsync(pageNumber, pageSize, roomStatus, roomNumber);
                return Ok(rooms);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                });
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomAsync(int id) 
        {
            try
            {
                var delete = await _roomService.DeleteRoomByIdASync(id);
                if (!delete)
                {
                    return NotFound(new { message = "Room not found." });
                }
                return Ok(new { message = "Room deleted successfully" });

            }catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal Server Error",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomAsync(int id,UpdateRoomDto updateRoomRequest)
        {
            try
            {
                var updated = await _roomService.UpdateRoomASync(id , updateRoomRequest);
                if (!updated)
                {
                    return NotFound(new { message = "Room not found" });
                }


                return Ok(new { message = "Room updated successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoomAsync(CreateRoomDto createRoomRequest)
        {
            try
            {
                var createdRoom = await _roomService.CreateRoomAsync(createRoomRequest);
                return Ok(new { message = "Room created successfully" });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                });
            }
        }
    }
}