using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public int TableId { get; set; }
        public string TableNumber { get; set; }
        public int BranchId { get; set; }
        public int WaiterId { get; set; }
        public string WaiterName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
