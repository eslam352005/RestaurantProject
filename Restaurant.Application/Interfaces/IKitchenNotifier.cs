using Restaurant.Application.DTOs.Inventory;
using Restaurant.Application.DTOs.Order;
using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface IKitchenNotifier
    {
        public Task NotifyOrderReceivedAsync(int branchId, OrderDto order);
        public Task MinimumThresholdAlertasync(int branchId, InventoryDto inventoryDto);
        public Task NotifyUpdatedOrderReceivedAsync(int branchId, OrderDto order);
        public Task NotifyOrderStatusChanged(int branchId, int orderId, OrderStatus status);
        public Task NotifyItemStatusChanged(int branchId, int orderId, int itemId, OrderItemStatus status);

    }
}
