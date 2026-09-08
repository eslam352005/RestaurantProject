using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public string TableNumber { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public TableStatus Status { get; set; } // Available, Occupied, Reserved
        public int BranchId { get; set; }
        public Branch Branch { get; set; } 
        public ICollection<Order> Orders { get; set; } 
    }
}
