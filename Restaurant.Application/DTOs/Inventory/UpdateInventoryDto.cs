using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Inventory
{
    public class UpdateInventoryDto
    {
        public string ItemName { get; set; }
        public decimal MinimumThreshold { get; set; }

    }
}
