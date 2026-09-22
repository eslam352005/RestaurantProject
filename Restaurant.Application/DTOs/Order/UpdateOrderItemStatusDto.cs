using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Order
{
    public class UpdateOrderItemStatusDto
    {
        public OrderItemStatus Status { get; set; }
    }
}
