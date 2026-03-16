using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Bookings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [Route("api/booking")]
    [ApiController]
    public class BookingController : Controller
    {
        private readonly IBookingService _service;
        private readonly IBookingCheckInOutService _bookingService;
        public BookingController(IBookingService service, IBookingCheckInOutService bookingService)
        {
            _service = service;
            _bookingService = bookingService;
        }


        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("getAllBookings")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }


        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("getBookingById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }


        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("updateBooking/{id}")]
        public async Task<IActionResult> Update(int id, UpdateBookingDto updateRequest)
        {
            await _service.UpdateAsync(id, updateRequest);

            return Ok(new
            {
                message = "Booking updated successfully."
            });
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpDelete("deleteBooking/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return Ok(new
            {
                message = "Booking deleted successfully."
            });
        }


        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("checkIn/{bookingId}")]
        public async Task<IActionResult> CheckIn(int bookingId)
        {
            var checkIn = await _bookingService.CheckInAsync(bookingId);
            return Ok(checkIn);
        }


        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("checkOut/{bookingId}")]
        public async Task<IActionResult> CheckOut(int bookingId)
        {
            var checkOut = await _bookingService.CheckOutAsync(bookingId);
            return Ok(checkOut);
        }


        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("getBookingsByRange")]
        public async Task<IActionResult> fetchBookingsByRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var res = await _service.GetBookingsByRange(start, end);
            return Ok(res);
        }


        [Authorize]
        [HttpPost("createBooking")]
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


        [Authorize]
        [HttpPost("{id}/cancelBooking")]
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
                    message = error.Message
                });
            }
        }
    }
}