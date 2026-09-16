using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Branch;
using Restaurant.Application.Interfaces;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class branchesController(IBranchService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetBranches()
        {
            var response = await _service.GetBranchesAsync();
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBranchById(int id)
        {
            var response = await _service.GetBranchByIdAsync(id);
            return Ok(response);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBranch([FromBody] CreateBranchDto dto)
        {
            var response = await _service.CreateBranchAsync(dto);
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBranch(int id, [FromBody] UpdateBranchDto dto)
        {
            var response = await _service.UpdateBranchAsync(id, dto);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            var response = await _service.DeleteBranchAsync(id);
            return Ok(response);
        }

    }
}
