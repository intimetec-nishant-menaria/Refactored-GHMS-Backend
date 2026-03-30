//using Microsoft.AspNetCore.Mvc;
//using guest_house_management_backend.Services.RoomType;

//namespace guest_house_management_backend.Controllers
//{
//    [Route("api/roomtypes")]
//    [ApiController]
//    public class RoomTypesController : ControllerBase
//    {
//        private readonly IRoomTypeService _service;

//        public RoomTypesController(IRoomTypeService service)
//        {
//            _service = service;
//        }

//        [HttpGet("/amenities")]
//        public async Task<IActionResult> GetAllAmenites()
//        {
//            var res = await _service.GetAllAminities();
//            return Ok(res);
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            return Ok(await _service.GetAllAsync());
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var result = await _service.GetByIdAsync(id);
//            if (result == null) return NotFound();
//            return Ok(result);
//        }
//    }
//}