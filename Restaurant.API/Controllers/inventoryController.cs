using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Inventory;
using Restaurant.Application.Interfaces;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class inventoryController(IInventoryService _service) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetInventoryAsync(int? branchid)
        {
            var result = await _service.GetInventoryAsync(branchid);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles ="Manager")]
        public async Task<IActionResult> CreateInventoryItemAsync(CreateInventoryDto dto)
        {
            var result = await _service.CreateInventoryItemAsync(dto);
            return Ok(result);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateInventoryItemAsync(int id, UpdateInventoryDto dto)
        {
            var result = await _service.UpdateInventoryItemAsync(id,dto);
            return Ok(result);
        }
        [HttpPatch("{id}/adjust-stock")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> AdjustStockAsync(int id, AdjustStockDto dto)
        {
            var result = await _service.AdjustStockAsync(id, dto);
            return Ok(result);
        }
        [HttpGet("low-stock-alerts")]
        [Authorize]
        public async Task<IActionResult> GetLowStockAlertsAsync(int? branchid)
        {
            var result = await _service.GetLowStockAlertsAsync(branchid);
            return Ok(result);
        }
    }
}
