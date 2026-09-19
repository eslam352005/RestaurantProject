using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Table
{
    public class TableDto
    {
        public int Id { get; set; }
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public int BranchId { get; set; }
    }
}
