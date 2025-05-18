using AutoMapper;
using electricity_provider_server_api.DTOs.Auth;
using electricity_provider_server_api.Models;
using electricity_provider_server_api.Repositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace electricity_provider_server_api.Services
{
    public class AuthService
    {
        private readonly AuthRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthService(AuthRepository userRepository, JwtTokenService jwtTokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        public async Task<(bool Success, string? ErrorMessage, string? Token, string? RefreshToken, object? Result)> Register(RegisterDto registerDto)
        {
            if (string.IsNullOrWhiteSpace(registerDto.Email) || string.IsNullOrWhiteSpace(registerDto.Password))
                return (false, "Email and password are required.", null, null, null);

            if (await _userRepository.DoesUserExistAsync(registerDto.Email))
                return (false, "User with this email already exists.", null, null, null);

            var newUser = _mapper.Map<User>(registerDto);
            newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            newUser.RefreshToken = GenerateRefreshToken();
            newUser.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await _userRepository.AddUserAsync(newUser);

            var token = _jwtTokenService.GenerateJwtToken(newUser);

            return (true, null, token, newUser.RefreshToken, new { user = new { newUser.Id, newUser.Email } });
        }

        public async Task<(bool Success, string ErrorMessage, string? Token, string? RefreshToken, object? Result)> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
                return (false, "Invalid credentials", null, null, null);

            var token = _jwtTokenService.GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUserAsync(user);

            return (true, null, token, refreshToken, new { user = new { user.Id, user.Email, user.Name } });
        }

        public async Task<(bool Success, string ErrorMessage, string? NewAccessToken, string? NewRefreshToken)> Refresh(string? oldRefreshToken)
        {
            if (string.IsNullOrEmpty(oldRefreshToken))
                return (false, "Refresh token missing", null, null);

            var user = await _userRepository.GetUserByRefreshTokenAsync(oldRefreshToken);

            if (user == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return (false, "Invalid or expired refresh token", null, null);

            var newAccessToken = _jwtTokenService.GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateUserAsync(user);

            return (true, "Token refreshed", newAccessToken, newRefreshToken);
        }

        public bool CheckAuth(string? token)
        {
            if (string.IsNullOrEmpty(token)) return false;

            var principal = _jwtTokenService.ValidateToken(token);
            return principal != null;
        }

        private string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }
    }
}
