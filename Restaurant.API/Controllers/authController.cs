using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Auth;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Enums;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class authController(IAuthService _service) : ControllerBase
    {

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _service.LoginAsync(loginDto);
            return Ok(result);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var result = await _service.RefreshTokenAsync(refreshTokenDto);
            return Ok(result);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _service.RegisterAsync(registerDto);
            return Ok(result);
        }
        [HttpPost("logout/{id}")]
        public async Task<IActionResult> Logout([FromRoute] string id)
        {
            var result = await _service.LogoutAsync(id);
            return Ok(result);
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("assign-role/{id}")]
        public async Task<IActionResult> AssignRole([FromRoute] string id, StaffRole role)
        {
            var result = await _service.AssignRoleAsync(id, role);
            return Ok(result);
        }
    }
}
