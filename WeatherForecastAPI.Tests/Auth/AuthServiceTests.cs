using Microsoft.Extensions.Configuration;
using Moq;
using WeatherForecast.Application.DTOs;
using WeatherForecast.Application.DTOs.Auth;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Exceptions;
using WeatherForecast.Domain.Interfaces;
using WeatherForecast.Infrastructure.Repositories;
using WeatherForecast.Infrastructure.Services;
using FluentAssertions;

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
        public async Task RegisterAsync_ShouldThrowException_WhenUserAlreadyExists()
        {
            var request = new RegisterRequest { Username = "duplicate", Password = "pass" };
            await _authService.RegisterAsync(request);

            var action = async () => await _authService.RegisterAsync(request);

            await action.Should().ThrowAsync<UserAlreadyExistsException>()
                .WithMessage("*duplicate*");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnToken_ForValidUser()
        {
            var register = new RegisterRequest { Username = "john", Password = "123" };
            await _authService.RegisterAsync(register);

            var login = new LoginRequest { Username = "john", Password = "123" };
            var result = await _authService.LoginAsync(login);

            result.Should().NotBeNull();
            result.Token.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenPasswordIsWrong()
        {
            var register = new RegisterRequest { Username = "sara", Password = "mypassword" };
            await _authService.RegisterAsync(register);

            var login = new LoginRequest { Username = "sara", Password = "wrongpass" };
            var action = async () => await _authService.LoginAsync(login);

            await action.Should().ThrowAsync<InvalidCredentialsException>();
        }

        [Fact]
        public async Task LoginAsync_ShouldThrowException_WhenUserNotFound()
        {
            var login = new LoginRequest { Username = "ghost", Password = "none" };
            var action = async () => await _authService.LoginAsync(login);

            await action.Should().ThrowAsync<InvalidCredentialsException>();
        }

        [Fact]
        public async Task RegisterAsync_ShouldHashPassword_NotStorePlainText()
        {
            var request = new RegisterRequest { Username = "secureuser", Password = "plainpass" };
            await _authService.RegisterAsync(request);

            var user = await _userRepository.GetByUsernameAsync("secureuser");

            user.Password.Should().NotBe("plainpass");
            user.Password.Length.Should().BeGreaterThan(10); // SHA256 => 64 hex chars
        }

        [Fact]
        public async Task RegisterAsync_ShouldBeCaseSensitive()
        {
            await _authService.RegisterAsync(new RegisterRequest { Username = "CaseUser", Password = "123" });

            var login = new LoginRequest { Username = "caseuser", Password = "123" };
            var action = async () => await _authService.LoginAsync(login);

            await action.Should().ThrowAsync<InvalidCredentialsException>();
        }

    }
}
