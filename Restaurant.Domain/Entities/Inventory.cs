using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class Inventory
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public decimal QuantityAvailable { get; set; }
        public string Unit { get; set; } = string.Empty; // kg, liter, piece 
        public decimal MinimumThreshold { get; set; }
        public int BranchId { get; set; }
        public Branch Branch { get; set; } = new Branch();
    }
}
