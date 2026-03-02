using guest_house_management_backend.DTOs;
using guest_house_management_backend.Enums;
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
            var name = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;
            var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
            var role = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var isActive = HttpContext.User.FindFirst("isActive")?.Value;

            return Ok(new
            {
                UserId =userId,
                Name = name,
                Email = email,
                Role = role,
                IsActive = isActive
            });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {
            try
            {
                var res = await _authService.LoginUserAsync(loginRequest);
                if (res.token == null)
                    return Unauthorized("Invalid Credentials");
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(5),
                    Path = "/"
                };
                Response.Cookies.Append("jwtToken", res.token, cookieOptions);
                var resUser = new UserResponseDto
                {
                    Id = res.user.Id,
                    Name = res.user.Name,
                    Email = res.user.Email,
                    Role = (RoleEnum)res.user.RoleId,
                    IsActive = res.user.IsActive
                };
                return Ok(new
                {
                    message = "Login successful",
                    user = resUser,
                });
            }
            catch(UnauthorizedAccessException ex)
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
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
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
        [Route("verifyForgetPassword")]
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
        [Route("resetPassword")]
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
