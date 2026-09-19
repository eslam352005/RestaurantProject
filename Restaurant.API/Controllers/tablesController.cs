using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Table;
using Restaurant.Application.Interfaces;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tablesController(ITableService tableService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllTables(int? branchId)
        {
            var response = await tableService.GetAllTablesAsync(branchId);
            return Ok(response);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTableById(int id)
        {
            var response = await tableService.GetTableByIdAsync(id);
            return Ok(response);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateTable(CreateTableDto tableDto)
        {
            var response = await tableService.CreateTableAsync(tableDto);
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTable(int id, UpdateTableDto tableDto)
        {
            var response = await tableService.UpdateTableAsync(id, tableDto);
            return Ok(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var response = await tableService.DeleteTableAsync(id);
            return Ok(response);
        }
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateTableStatus(int id, UpdateTableStatusDto statusDto)
        {
            var response = await tableService.UpdateTableStatusAsync(id, statusDto);
            return Ok(response);
        }
        
        
    }
}
