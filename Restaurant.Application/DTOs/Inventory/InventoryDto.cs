using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Inventory
{
    public class InventoryDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public decimal QuantityAvailable { get; set; }
        public string Unit { get; set; }
        public decimal MinimumThreshold { get; set; }
        public int BranchId { get; set; }
        public bool IsLowStock { get; set; } = false;
    }
}
