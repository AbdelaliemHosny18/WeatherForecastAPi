using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherForecast.Application.Interfaces;

namespace WeatherForecast.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest("City is required");

            var weather = await _weatherService.GetWeatherAsync(city);
            return Ok(weather);
        }
    }
}
