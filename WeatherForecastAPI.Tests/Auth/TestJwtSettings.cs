using Microsoft.Extensions.Options;
using WeatherForecast.Application.Models;

namespace WeatherForecastAPI.Tests.Auth
{
    public class TestJwtSettings : IOptions<JwtSettings>
    {
        public JwtSettings Value => new JwtSettings
        {
            Key = "ThisIsASecureTestKeyThatIsAtLeast32Chars",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            DurationInMinutes = 60
        };
    }
}
