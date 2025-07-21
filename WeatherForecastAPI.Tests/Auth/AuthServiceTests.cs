using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.DTOs;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Interfaces;
using WeatherForecast.Infrastructure.Repositories;
using WeatherForecast.Infrastructure.Services;

namespace WeatherForecastAPI.Tests.Auth
{
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly Mock<IConfiguration> _mockConfig;

        public AuthServiceTests()
        {
            _userRepository = new InMemoryUserRepository();

            _mockConfig = new Mock<IConfiguration>();

            // Setup the JWT section in configuration
            _mockConfig.Setup(c => c["Jwt:Key"]).Returns("YourSuperLongTestKeyThatIsAtLeast32Chars");
            _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("TestIssuer");
            _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("TestAudience");
            _mockConfig.Setup(c => c["Jwt:DurationInMinutes"]).Returns("60");

            _authService = new AuthService(_userRepository, _mockConfig.Object);
        }

        [Fact]
        public async Task RegisterAsync_ShouldRegisterNewUser()
        {
            var request = new RegisterRequest
            {
                Username = "testuser",
                Password = "password"
            };

            var result = await _authService.RegisterAsync(request);

            result.Should().NotBeNull();
            result.Username.Should().Be("testuser");
            result.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_ForValidUser()
        {
            // Arrange
            var request = new RegisterRequest { Username = "john", Password = "123" };
            await _authService.RegisterAsync(request);

            var login = new LoginRequest { Username = "john", Password = "123" };

            // Act
            var result = await _authService.LoginAsync(login);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
        }
    }
}
