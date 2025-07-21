using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Application.DTOs;
using WeatherForecast.Application.Interfaces;

namespace WeatherForecast.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and Password are required.");

            try
            {
                var result = await _authService.RegisterAsync(request);
                return Ok(result); // 200 OK
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("already exists"))
                    return Conflict("User already exists."); // 409 Conflict

                return StatusCode(500, "Something went wrong."); // 500 Internal Server Error
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and Password are required.");

            try
            {
                var result = await _authService.LoginAsync(request);
                return Ok(result); // 200 OK
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid credentials"))
                    return Unauthorized("Invalid username or password."); // 401 Unauthorized

                return StatusCode(500, "Something went wrong."); // 500 Internal Server Error
            }
        }
    }

}
