using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Guest;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [Route("api/guest")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        public readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGuests()
        {
            var guest =  await _guestService.GetAllGuestsAsync();
            return Ok(guest);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuestResponseDto>> GetById(int id)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);
            return Ok(guest);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<GuestResponseDto>>> Search([FromQuery] string search)
        {
            var guests = await _guestService.SearchGuestsAsync(search);
            return Ok(guests);
        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateGuestDto createRequest)
        {
            await _guestService.CreateGuestAsync(createRequest);

            return Ok(new
            {
                message = "Guest created successfully."
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateGuestDto updateRequest)
        {
            await _guestService.UpdateGuestAsync(id, updateRequest);

            return Ok(new
            {
                message = "Guest updated successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _guestService.DeleteGuestAsync(id);

            return Ok(new
            {
                message = "Guest deleted successfully."
            });
        }
    }
}
