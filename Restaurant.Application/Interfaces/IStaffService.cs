using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.DTOs.Staff;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface IStaffService
    {
        public Task<ResponseDto<StaffDto>> GetStaffByIdAsync(int id);
        public Task<ResponseDto<IEnumerable<StaffDto>>> GetStaffsAsync(int? branchId);
        public Task<ResponseDto<StaffDto>> CreateStaffAsync(CreateStaffDto staffDto);
        public Task<ResponseDto<StaffDto>> UpdateStaffAsync(int id, UpdateStaffDto staffDto);
        public Task<ResponseDto<bool>> DeleteStaffAsync(int id);
    }
}
