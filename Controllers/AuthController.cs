using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

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

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, 
                Secure = false,   
                SameSite = SameSiteMode.Strict, 
                Expires = DateTime.UtcNow.AddDays(5)
            };

            Response.Cookies.Append("jwtToken", token, cookieOptions);

            return Ok(new { token});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        { 
            var result = await _authService.RegisterAsync(dto);

            if(!result)
                return Conflict(new { message = "User already exists." });

            return Ok(new { message = "Registration successful." });
        }

        [Authorize]
        [HttpPost("logout")]
       public IActionResult Logout()
       {
            Response.Cookies.Delete("jwtToken");
            return Ok(new { message = "Loggoed out successfully" });
       }


        [Authorize]
        [HttpPost("change-password")]
        public  async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            var UserIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (UserIdClaim == null)
                return Unauthorized();

            int UserId = int.Parse(UserIdClaim.Value);  
            var result = await _authService.ChangePassword(UserId,changePassword);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }

        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPassword)
        {
            await _authService.ForgetPasswordAsync(forgetPassword);
            return Ok("A reset email has been sent");
        }

        [HttpGet("verify-forget-password")]
        public async Task<IActionResult> VerifyResetToken(string email ,string token)
        {
            var IsValid = await _authService.VerifyResetTokenAsync(email, token);

            if (!IsValid)
                return BadRequest("Invalid or expired token.");

            return Ok("Token is valid.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);

            if (!result)
                return BadRequest("Invalid or expired token.");

            return Ok("Password reset successful.");
        }

    }
}
