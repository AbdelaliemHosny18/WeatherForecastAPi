using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Infrastructure.Services;
using Microsoft.Extensions.Caching.Memory;
using FluentAssertions;


namespace WeatherForecastAPI.Tests.Weather
{
    public class WeatherServiceTests
    {
        private readonly IWeatherService _weatherService;

        public WeatherServiceTests()
        {
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            _weatherService = new WeatherService(memoryCache);
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldReturnData()
        {
            var city = "Cairo";
            var result = await _weatherService.GetWeatherAsync(city);

            result.Should().NotBeNull();
            result.City.Should().Be("Cairo");
            result.TemperatureC.Should().BeInRange(-10, 40); // mock temp
        }

        [Fact]
        public async Task GetWeatherAsync_ShouldReturnCachedData_OnSecondCall()
        {
            var city = "Alexandria";

            var firstCall = await _weatherService.GetWeatherAsync(city);
            var secondCall = await _weatherService.GetWeatherAsync(city);

            firstCall.Should().BeEquivalentTo(secondCall); // Cached
        }
    }
}
