using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Report
{
    public class SalesReportDto
    {
        public DateTime Date { get; set; }
        public decimal TotalSales { get; set; }
        public int OrdersCount { get; set; }
    }
}
