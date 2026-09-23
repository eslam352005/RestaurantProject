using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Inventory
{
    public class CreateInventoryDto
    {
       
        public string ItemName { get; set; }
        public decimal QuantityAvailable { get; set; }
        public string Unit { get; set; }
        public decimal MinimumThreshold { get; set; }
        public int BranchId { get; set; }
        
    }
}
