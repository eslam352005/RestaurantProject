using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = new Order();
        public int MenuItemId { get; set; }
        public MenuItem MenuItem { get; set; } = new MenuItem();
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; } = string.Empty;
        public OrderItemStatus Status { get; set; } // Pending, Preparing, Ready
    }
}
