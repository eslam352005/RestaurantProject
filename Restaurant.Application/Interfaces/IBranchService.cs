using Restaurant.Application.DTOs.Branch;
using Restaurant.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface IBranchService
    {
        public Task<ResponseDto<BranchDto>> GetBranchByIdAsync(int id);
        public Task<ResponseDto<IEnumerable<BranchDto>>> GetBranchesAsync();
        public Task<ResponseDto<BranchDto>> CreateBranchAsync(CreateBranchDto dto);
        public Task<ResponseDto<BranchDto>> UpdateBranch(int id, UpdateBranchDto dto);
        public Task<ResponseDto<bool>> DeleteBranch(int id);
    }
}
