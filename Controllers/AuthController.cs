using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace guest_house_management_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.LoginAsync(dto);

            if (token == null)
                return Unauthorized("invalid Credentials");

            return Ok(new { token});
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(RegisterDto dto)
        { 
            var result = await _authService.RegisterAsync(dto);

            if(!result)
                return Conflict(new { message = "User already exists." });

            return Ok(new { message = "Registration successful." });
        }

    }
}
