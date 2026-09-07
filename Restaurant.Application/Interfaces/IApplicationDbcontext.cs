using Microsoft.EntityFrameworkCore;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface IApplicationDbcontext
    {

        DbSet<Branch> Branches { get;  }
        DbSet<Staff> Staff { get;  }
        DbSet<Category> Categories { get;  }
        DbSet<Inventory> Inventories { get;  }
        DbSet<MenuItem> MenuItems { get;  }
        DbSet<Order> Orders { get;  }
        DbSet<OrderItem> OrderItems { get;  }
        DbSet<Table> Tables { get;  }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
