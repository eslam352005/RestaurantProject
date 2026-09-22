using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Order
{
    public class CreateOrderDto
    {
        public int TableId { get; set; }
        public int BranchId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
    }
}
