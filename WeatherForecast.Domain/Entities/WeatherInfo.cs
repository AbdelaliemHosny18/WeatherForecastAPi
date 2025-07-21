namespace WeatherForecast.Domain.Entities
{
    public class WeatherInfo
    {
        public string City { get; set; } = default!;
        public double TemperatureC { get; set; }
        public string Summary { get; set; } = default!;
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
