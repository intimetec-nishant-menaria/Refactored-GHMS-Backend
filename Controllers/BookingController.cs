using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Bookings;
using Microsoft.AspNetCore.Mvc;

namespace guest_house_management_backend.Controllers
{
    [Route("api/bookings")]
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

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto createRequest)
        {
            var booking = await _service.CreateAsync(createRequest);
            return Ok(new
            {
                message = "Booking Created successfully.",
                data = booking
            });
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

        [HttpPost("{bookingId}/checkin")]
        public async Task<IActionResult> CheckIn(int bookingId)
        {
            var checkIn = await _bookingService.CheckInAsync(bookingId);
            return Ok(checkIn);
        }

        [HttpPost("{bookingId}/checkout")]
        public async Task<IActionResult> CheckOut(int bookingId)
        {
            var checkOut = await _bookingService.CheckOutAsync(bookingId);
            return Ok(checkOut);
        }
    }
}
