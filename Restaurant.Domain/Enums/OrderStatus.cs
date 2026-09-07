using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Enums
{
    public enum OrderStatus
    {
        Pending = 0,
        Preparing = 1,
        Ready = 2,
        Served = 3,
        Cancelled = 4,
    }
}
