using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Order
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
    }
}
