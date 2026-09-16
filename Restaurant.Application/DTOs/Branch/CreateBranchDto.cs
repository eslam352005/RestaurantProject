using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.DTOs.Branch
{
    public class CreateBranchDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
}
