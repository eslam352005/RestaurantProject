using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Report
{
    public class TopItemDto
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public int TotalOrdered { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
