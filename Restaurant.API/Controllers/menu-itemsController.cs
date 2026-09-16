using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.MenuItem;
using Restaurant.Application.Interfaces;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class menu_itemsController(IMenuItemService _service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetMenuItems(int? branchId, int? categoryId)
        {
            var menuItems = await _service.GetMenuItemsAsync(branchId, categoryId);
            return Ok(menuItems);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var menuItem = await _service.GetMenuItemByIdAsync(id);
            return Ok(menuItem);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> CreateMenuItem([FromBody] CreateMenuItemDto dto)
        {
            var menuItem = await _service.CreateMenuItemAsync(dto);
            return Ok(menuItem);
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]

        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] UpdateMenuItemDto dto)
        {
            var menuItem = await _service.UpdateMenuItem(id, dto);
            return Ok(menuItem);
        }
        [HttpPatch("{id}/availability")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateMenuItemAvailability(int id, [FromBody] UpdateMenuItemAvailabilityDto dto)
        {
            var menuItem = await _service.UpdateMenuItemAvailability(id, dto);
            return Ok(menuItem);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var result = await _service.DeleteMenuItem(id);
            return Ok(result);
        }
        [HttpPost("{id}/upload-image")]
        [Consumes("multipart/form-data")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UploadMenuItemImage(int id,  IFormFile image)
        {
            var result = await _service.UploadImageAsync(id, image);
            return Ok(result);
        }
    }
}
