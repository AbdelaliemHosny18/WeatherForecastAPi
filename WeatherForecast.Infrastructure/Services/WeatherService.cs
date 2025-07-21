using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.DTOs.Weather;
using WeatherForecast.Application.Interfaces;

namespace WeatherForecast.Infrastructure.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IMemoryCache _cache;
        private static readonly string[] Summaries = new[]
        {
        "Sunny", "Cloudy", "Rainy", "Windy", "Snowy", "Misty"
    };

        public WeatherService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<WeatherResponse> GetWeatherAsync(string city)
        {
            // Try to get from cache
            if (_cache.TryGetValue(city.ToLower(), out WeatherResponse cachedWeather))
            {
                return cachedWeather;
            }

            // Simulate mock delay
            await Task.Delay(500);

            // Mocked random data
            var response = new WeatherResponse
            {
                City = city,
                TemperatureC = new Random().Next(-10, 40),
                Summary = Summaries[new Random().Next(Summaries.Length)],
                Date = DateTime.UtcNow
            };

            // Cache for 5 minutes
            _cache.Set(city.ToLower(), response, TimeSpan.FromMinutes(5));

            return response;
        }
    }

}
