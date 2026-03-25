using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Room;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpPut("updateRoomStatus/{id}")]
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

        [HttpGet("getRoomStatus/{id}")]
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

        [HttpGet("getSummary")]
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

        [HttpDelete("deleteRoom/{id}")]
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

        [HttpPut("updateRoom/{id}")]
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

        [HttpPost("createRoom")]
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
        [HttpGet("getAllRooms")]
        public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAllRoomsAsync([FromQuery] int pageNumber , [FromQuery] int pageSize , [FromQuery] int roomStatus , [FromQuery] int roomType)
        {
            try
            {
                var rooms = await _roomService.getAllRoomAsync(pageNumber , pageSize , roomStatus , roomType);
                return Ok(rooms);
            }catch(Exception ex)
            {
                return StatusCode(500,new 
                {
                    message = ex.Message,
                });
            }
        }
        
    }
}