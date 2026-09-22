using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Order
{
    public class UpdateOrderItemsDto
    {
        public List<CreateOrderItemDto> Items { get; set; }
    }
}
