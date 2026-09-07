using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ICollection<Table> Tables { get; set; } = new List<Table>();
        public ICollection<Staff> Staff { get; set; } = new List<Staff>();
        public ICollection<Inventory> InventoryItems { get; set; } = new List<Inventory>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
