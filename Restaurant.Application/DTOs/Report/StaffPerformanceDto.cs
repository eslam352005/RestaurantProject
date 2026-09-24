using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Report
{
    public class StaffPerformanceDto
    {
        public int StaffId { get; set; }
        public string FullName { get; set; }
        public int OrdersHandled  { get; set; }
        public decimal TotalSalesHandled { get; set; }
    }
}
