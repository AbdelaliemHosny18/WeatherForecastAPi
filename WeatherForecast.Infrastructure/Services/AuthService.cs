using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WeatherForecast.Application.DTOs;
using WeatherForecast.Application.DTOs.Auth;
using WeatherForecast.Application.Interfaces;
using WeatherForecast.Domain.Entities;
using WeatherForecast.Domain.Interfaces;

namespace WeatherForecast.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            var existing = await _userRepo.GetByUsernameAsync(request.Username);
            if (existing is not null)
                throw new Exception("User already exists");

            var user = new User
            {
                Username = request.Username,
                Password = request.Password 
            };

            await _userRepo.AddAsync(user);
            return new AuthResponse { Token = GenerateJwtToken(user) ,Username= request.Username };
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _userRepo.GetByUsernameAsync(request.Username);
            if (user == null || user.Password != request.Password)
                throw new Exception("Invalid credentials");

            return new AuthResponse { Token = GenerateJwtToken(user) };
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
