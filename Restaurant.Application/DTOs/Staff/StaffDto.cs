using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Staff
{
    public class StaffDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
    }
}
