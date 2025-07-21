using WeatherForecast.Application.DTOs.Weather;

namespace WeatherForecast.Application.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponse> GetWeatherAsync(string city);
    }
}
