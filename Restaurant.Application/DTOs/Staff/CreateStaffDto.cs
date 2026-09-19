using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Staff
{
    public class CreateStaffDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public StaffRole Role { get; set; }
        public int BranchId { get; set; }
    }
}
