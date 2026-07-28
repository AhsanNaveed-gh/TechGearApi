using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Models;
using TechGearAPI.Services;

namespace TechGearAPI.Controllers
{
        [ApiController]
        [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            var user = await _userService.RegisterUser(dto);
            return Ok(user);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            var token = await _userService.LoginUser(dto);

            return Ok(token);
        }
    }
}
