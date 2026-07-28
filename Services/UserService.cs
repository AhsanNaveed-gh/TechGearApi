using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.DTOs;
using TechGearAPI.Models;
using TechGearAPI.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using BCrypt.Net;

namespace TechGearAPI.Services
{
    public class UserService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
        }
        public async Task<UserResponseDto> RegisterUser(RegisterUserDto dto)
        {
            var emailExists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
            if (emailExists)
            {
                throw new Exception("Email already exists");
            }
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12),
                Role = "Customer"
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };
        }
        public async Task<LoginResponseDto> LoginUser(LoginUserDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
            {
                throw new Exception("Invalid email or password");
            }
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new Exception("Invalid email or password");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
            );
            var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
            );
            var expires = DateTime.UtcNow.AddHours(1);
            var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
            );
            return new LoginResponseDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = dto.Email,
                Role = user.Role,
                ExpiresAt = expires
            };
            
        }
    }
}
