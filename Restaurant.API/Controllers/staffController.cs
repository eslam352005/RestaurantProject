using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Staff;
using Restaurant.Application.Interfaces;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class staffController(IStaffService _service) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStaffById(int id)
        {
            var response = await _service.GetStaffByIdAsync(id);
            if (response.Data == null)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetStaffs([FromQuery] int? branchId)
        {
            var response = await _service.GetStaffsAsync(branchId);
            return Ok(response);
        }
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDto staffDto)
        {
            var response = await _service.CreateStaffAsync(staffDto);
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaff(int id, [FromBody] UpdateStaffDto staffDto)
        {
            var response = await _service.UpdateStaffAsync(id, staffDto);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var response = await _service.DeleteStaffAsync(id);
            return Ok(response);
        }
    }
}
