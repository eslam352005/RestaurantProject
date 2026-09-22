using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.DTOs.Order;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Enums;
using System.Security.Claims;

namespace Restaurant.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ordersController(IOrderservice _orderservice) : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrdersAsync(int? branchId, OrderStatus? status, DateTime? date)
        {
            var result = await _orderservice.GetOrdersAsync(branchId, status, date);
            return Ok(result);
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrderByIdAsync(int id)
        {
            var result = await _orderservice.GetOrderByIdAsync(id);
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles ="Waiter")]
        public async Task<IActionResult> CreateOrderAsync( CreateOrderDto createOrderDto)
        {
            var waiterId = int.Parse(User.FindFirstValue("StaffId")!);
            var result = await _orderservice.CreateOrderAsync(waiterId,createOrderDto);
            return Ok(result);
        }
        [HttpPut("{id}/items")]
        [Authorize(Roles = "Waiter")]
        public async Task<IActionResult> UpdateOrderItemsAsync(int id, UpdateOrderItemsDto itemsDto)
        {
            var result = await _orderservice.UpdateOrderItemsAsync(id, itemsDto);
            return Ok(result);
        }
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Waiter,Chef")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto statusDto)
        {
            var result = await _orderservice.ChangeOrderStatusAsync(id, statusDto);
            return Ok(result);
        }
        [HttpPatch("{orderid}/items/{itemId}/status")]
        [Authorize(Roles = "Chef")]
        public async Task<IActionResult> UpdateOrderItemStatusAsync(int orderid, int itemId, UpdateOrderItemStatusDto statusDto)
        {
            var result = await _orderservice.ChangeOrderItemStatusAsync(orderid,itemId, statusDto);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrderAsync(int id)
        {
            var result = await _orderservice.DeleteOrderAsync(id);
            return Ok(result);
        }
    }
}
