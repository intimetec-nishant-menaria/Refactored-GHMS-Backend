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
        public BookingController(IBookingService service)
        {
            _service = service;
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
    }
}
