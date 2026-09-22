using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Order
{
    public class UpdateOrderStatusDto
    {
        public OrderStatus Status { get; set; }
    }
}
