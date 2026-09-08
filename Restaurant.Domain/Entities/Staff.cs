using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Domain.Entities
{
    public class Staff
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty; // FK to Identity User
        public string FullName { get; set; } = string.Empty;
        public StaffRole Role { get; set; } // Admin, Manager, Chef, Waiter
        public int BranchId { get; set; }
        public Branch Branch { get; set; } 
        public ApplicationUser ApplicationUser { get; set; } 
    }
}
