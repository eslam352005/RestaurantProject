using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Table
{
    public class CreateTableDto
    {
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
        public int BranchId { get; set; }
    }
}
