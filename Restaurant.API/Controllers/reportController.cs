using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Interfaces;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reportController(IReportService _service) : ControllerBase
    {
        [Authorize(Roles ="Manager,Admin")]
        [HttpGet("sales")]
        public async Task<IActionResult> GetSalesReportAsync(int? branchId, DateTime? from = null, DateTime? to = null)
        {
            var result = await _service.GetSalesReportAsync(branchId, from, to);
            return Ok(result);
        }
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet("top-items")]
        public async Task<IActionResult> GetTopItemsReportAsync(int? branchId, int limit = 5)
        {
            var result = await _service.GetTopItemsReportAsync(branchId,limit);
            return Ok(result);
        }
        [Authorize(Roles = "Manager,Admin")]
        [HttpGet("staff-performance")]
        public async Task<IActionResult> GetStaffPerformanceReportAsync(int? branchId)
        {
            var result = await _service.GetStaffPerformanceReportAsync(branchId);
            return Ok(result);
        }
    }
}
