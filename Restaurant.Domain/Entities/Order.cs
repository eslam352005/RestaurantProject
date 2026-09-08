using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; } // Pending, Preparing, Ready, Served, Cancelled
        public int TableId { get; set; }
        public Table Table { get; set; } 
        public int BranchId { get; set; }
        public Branch Branch { get; set; } 
        public int WaiterId { get; set; }
        public Staff Waiter { get; set; } 
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> Items { get; set; } 
    }
}
