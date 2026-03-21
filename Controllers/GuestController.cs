using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Guest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [Route("api/guest")]
    [ApiController]
    [Authorize(Roles = "Admin,Staff")]
    public class GuestController : ControllerBase
    {
        public readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpGet("getAllGuests")]
        public async Task<IActionResult> GetAllGuests([FromQuery] int pageNumber , [FromQuery] int pageSize , [FromQuery] string? searchUser)
        {
            var guest =  await _guestService.GetAllGuestsAsync(pageNumber , pageSize,searchUser);
            return Ok(guest);
        }

        [HttpGet("getGuestById/{id}")]
        public async Task<ActionResult<GuestResponseDto>> GetById(int id)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);
            return Ok(guest);
        }

        [HttpGet("searchGuests")]
        public async Task<ActionResult<IEnumerable<GuestResponseDto>>> Search([FromQuery] string search)
        {
            var guests = await _guestService.SearchGuestsAsync(search);
            return Ok(guests);
        }

        [HttpPost("createGuest")]
        public async Task<IActionResult> Create( CreateGuestDto createRequest)
        {
            await _guestService.CreateGuestAsync(createRequest);

            return Ok(new
            {
                message = "Guest created successfully."
            });
        }

        [HttpPut("{id}/updateGuest")]
        public async Task<IActionResult> Update(int id, UpdateGuestDto updateRequest)
        {
            try
            {
                await _guestService.UpdateGuestAsync(id, updateRequest);

                return Ok(new
                {
                    message = "Guest updated successfully."
                });
            }catch(InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    message = ex.Message,
                });
            }
        }

        [HttpDelete("{id}/deleteGuest")]
        public async Task<IActionResult> Delete(int id)
        {
            await _guestService.DeleteGuestAsync(id);

            return Ok(new
            {
                message = "Guest deleted successfully."
            });
        }

        [HttpGet("booking-history/{guestId}")]
        public async Task<IActionResult> GetGuestBookingHistory(int guestId, [FromQuery] GuestBookingHistoryQueryDto queryDto)
        {
            var result = await _guestService
                .GetGuestBookingHistoryAsync(guestId, queryDto);
            return Ok(result);
        }
    }
}
