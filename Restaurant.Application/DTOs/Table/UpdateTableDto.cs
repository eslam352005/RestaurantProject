using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Table
{
    public class UpdateTableDto
    {
        public string TableNumber { get; set; }
        public int Capacity { get; set; }
    }
}
