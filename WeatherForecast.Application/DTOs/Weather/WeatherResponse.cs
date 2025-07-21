using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Application.DTOs.Weather
{
    public class WeatherResponse
    {
        public string City { get; set; } = default!;
        public double TemperatureC { get; set; }
        public string Summary { get; set; } = default!;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
