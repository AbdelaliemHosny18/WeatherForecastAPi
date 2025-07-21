using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Application.Models;
using WeatherForecast.Domain.Interfaces;
using WeatherForecast.Infrastructure.Repositories;
using WeatherForecast.Infrastructure.Services;

namespace WeatherForecast.API.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services,
         WebApplicationBuilder builder)
        {

            // Add DI for services
            services.AddSingleton<IUserRepository, InMemoryUserRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IWeatherService, WeatherService>();
            services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
            // JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(jwtSettings.Key);
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            // Add AddMemoryCache
            services.AddMemoryCache();

            // Add services to the container.
            // Swagger & minimal extras
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }
    }
}
