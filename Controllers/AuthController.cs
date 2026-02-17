using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace guest_house_management_backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Authorize]
        [HttpGet]
        [Route("me")]
        public IActionResult GetMe()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new
            {
                UserId =userId,
                Email = email,
                Role = role
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {
            try
            {
                var token = await _authService.LoginUserAsync(loginRequest);
                if (token == null)
                    return Unauthorized("Invalid Credentials");
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = false,
                    Secure = false,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(5)
                };
                Response.Cookies.Append("jwtToken", token, cookieOptions);
                return Ok("Login successful");
            }catch(UnauthorizedAccessException ex)
            {
                return Unauthorized(new {message = ex.Message});
            }catch(Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto registrationRequest)
        {
            try
            {
                await _authService.RegisterUserAsync(registrationRequest);
                return Ok(new { message = "Registration successful." });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            try
            {
                Response.Cookies.Delete("jwtToken");
                return Ok(new { message = "Logged out successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("changePassword")]
        public async Task<IActionResult> ChangeUserPassword(ChangePasswordDto changePasswordRequest)
        {
            try
            {
                var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                    return Unauthorized(new { message = "Invalid user." });

                var result = await _authService.ChangeUserPassword(userId, changePasswordRequest);
                if (!result.Success)
                    return BadRequest(new { message = result.Message });

                return Ok(new { message = result.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("forgetPassword")]
        public async Task<IActionResult> ForgetUserPassword(ForgetPasswordDto forgetPasswordRequest)
        {
            try
            {
                await _authService.ForgetUserPasswordAsync(forgetPasswordRequest);
                return Ok(new { message = "A reset email has been sent." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong." });
            }
        }

        [HttpGet]
        [Route("verify-forget-password")]
        public async Task<IActionResult> VerifyResetToken(string email, string token)
        {
            try
            {
                await _authService.VerifyResetTokenAsync(email, token);
                return Ok(new { message = "Token is valid." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong." });
            }
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordRequest)
        {
            try
            {
                await _authService.ResetPasswordAsync(resetPasswordRequest);
                return Ok(new { message = "Password reset successful." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Something went wrong." });
            }
        }

    }
}
