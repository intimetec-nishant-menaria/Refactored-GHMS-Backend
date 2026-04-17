using guest_house_management_backend.DTOs;
using guest_house_management_backend.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace guest_house_management_backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IDistributedCache _distributedCache;

        public AuthController(IAuthService authService , IDistributedCache distributedCache)
        {
            _authService = authService;
            _distributedCache = distributedCache;
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { message = "No refresh token provided." });
            }

            try
            {
                var (newAccessToken , userDetails ) = await _authService.AccessTokenAsync( refreshToken);

                if (newAccessToken == null ||  userDetails == null)
                {
                    return Unauthorized(new { message = "Invalid session" });
                }

                return Ok(new { accessToken = newAccessToken , user = userDetails });
            }
            catch(Exception)
            {
                return Unauthorized(new { message = "Token refresh failed." });
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginDto loginRequest)
        {
            try
            {
                var ( accessToken , refreshToken , user) = await _authService.LoginUserAsync(loginRequest);
                if (accessToken == null || refreshToken==null)
                    return Unauthorized("Invalid Credentials");

                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(10),
                    Path = "/"
                };
                Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
                return Ok(new
                {
                    message = "Login successful",
                    token = accessToken,
                    user = new UserResponseDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Role = user.Role.RoleName,
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt
                    }
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

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            try
            {
                Response.Cookies.Delete("accessToken");
                Response.Cookies.Delete("refreshToken", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None 
                });

                if (!string.IsNullOrEmpty(userId))
                {
                    await _distributedCache.RemoveAsync($"refresh_{userId}");
                }

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
        public async Task<IActionResult> ResetPassword([FromQuery] string email, [FromQuery] string token,ResetPasswordDto resetPasswordRequest)
        {
            try
            {
                await _authService.ResetPasswordAsync(email , token ,resetPasswordRequest);
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
