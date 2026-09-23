using Microsoft.AspNetCore.SignalR;
using Restaurant.Application.DTOs.Inventory;
using Restaurant.Application.DTOs.Order;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Enums;

namespace Restaurant.API.Hubs
{
    public class KitcheNotifier(IHubContext<KitchenHub> _hubContext) : IKitchenNotifier
    {
        public async Task NotifyItemStatusChanged(int branchId, int orderId, int itemId, OrderItemStatus status)
        {
            await _hubContext.Clients.Group($"branch_{branchId}").SendAsync("ItemStatusChanged", new { orderId, itemId, status });
        }

        public async Task NotifyOrderReceivedAsync(int branchId, OrderDto order)
        {
            await _hubContext.Clients.Group($"branch_{branchId}").SendAsync("OrderReceived", order);
        }
        public async Task MinimumThresholdAlertasync(int branchId,InventoryDto inventoryDto)
        {
            await _hubContext.Clients.Group($"branch_{branchId}").SendAsync("MinimumThresholdAlert", inventoryDto);
        }

        public async Task NotifyOrderStatusChanged(int branchId, int orderId, OrderStatus status)
        {
            await _hubContext.Clients.Group($"branch_{branchId}").SendAsync("OrderStatusChanged", new { orderId, status });
        }

        public async Task NotifyUpdatedOrderReceivedAsync(int branchId, OrderDto order)
        {
            await _hubContext.Clients.Group($"branch_{branchId}").SendAsync("UpdatedOrderReceived", order);
        }
    }
}
