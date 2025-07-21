using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherForecast.Application.DTOs.Auth
{
    public class AuthResponse
    {
        public string Token { get; set; } = default!;
        public string Username { get; set; } = default!;

    }
}
