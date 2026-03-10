using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    //[Authorize]
    [Route("api/booking")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _service;

        public BookingController(IBookingService service)
        {
            _service = service;
        }

        [HttpPost("available")]
        public async Task<IActionResult> GetAllAvailableRooms(RoomAvaiblityRequestDto roomAvaiblityRequest)
        {
            var res = await _service.GetAllAvailableRooms(roomAvaiblityRequest);
            return Ok(res);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("range")]
        public async Task<IActionResult> fetchBookingsByRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var res = await _service.GetBookingsByRange(start, end);
            return Ok(res);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto createRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _service.CreateAsync(createRequest);
                return Ok(new
                {
                    message = "Booking Created successfully."
                });
            }catch(InvalidOperationException error)
            {
                return BadRequest(new
                {
                    message = "The selected room is not available for the chosen dates."
                });
            }catch(KeyNotFoundException error)
            {
                return BadRequest(new
                {
                    message = "Room not Found"
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookingDto updateRequest)
        {
            await _service.UpdateAsync(id, updateRequest);

            return Ok(new
            {
                message = "Booking updated successfully."
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                message = "Booking deleted successfully."
            });
        }

        [HttpPost("cancel/{id}")]
        public async Task<IActionResult> CancelBooking(int id)
        {
            try
            {
                await _service.CancelBooking(id);
                return Ok(new
                {
                    message = "Booking Cancelled successfully."
                });

            }catch(KeyNotFoundException error)
            {
                return BadRequest(new
                {
                    message = "something went wrong"
                });
            }
        }
    }
}