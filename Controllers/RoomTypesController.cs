using Microsoft.AspNetCore.Mvc;
using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.RoomType;

namespace guest_house_management_backend.Controllers
{
    [Route("api/roomtypes")]
    [ApiController]
    public class RoomTypesController : ControllerBase
    {
        private readonly IRoomTypeService _service;

        public RoomTypesController(IRoomTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateRoomTypeDto dto)
        //{
        //    await _service.CreateAsync(dto);
        //    return Ok("Room Type Created Successfully");
        //}

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(int id, CreateRoomTypeDto dto)
        //{
        //    await _service.UpdateAsync(id, dto);
        //    return Ok("Room Type Updated Successfully");
        //}

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    await _service.DeleteAsync(id);
        //    return Ok("Room Type Deleted Successfully");
        //}
    }
}