using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Branch;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Services
{
    public class BranchService(IApplicationDbcontext _context,IMapper _mapper) : IBranchService
    {
        public async Task<ResponseDto<BranchDto>> CreateBranchAsync(CreateBranchDto dto)
        {
            var branch = _mapper.Map<Branch>(dto);
            await _context.Branches.AddAsync(branch);
            await _context.SaveChangesAsync();
            var branchDto = _mapper.Map<BranchDto>(branch);
            return ResponseHandler.Success(branchDto, "Branch created successfully");
        }

        public async Task<ResponseDto<bool>> DeleteBranchAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if(branch == null)
            {
                return ResponseHandler.NotFound<bool>($"Branch with ID {id} not found");
            }
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            return ResponseHandler.Success(true, "Branch deleted successfully");
        }

        public async Task<ResponseDto<BranchDto>> GetBranchByIdAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if(branch == null)
            {
                return ResponseHandler.NotFound<BranchDto>($"Branch with ID {id} not found");
            }
           var branchDto = _mapper.Map<BranchDto>(branch);
            return ResponseHandler.Success(branchDto, "Branch retrieved successfully");
        }

        public async Task<ResponseDto<IEnumerable<BranchDto>>> GetBranchesAsync()
        {
            var branches = await _context.Branches.ToListAsync();
            var branchDtos = _mapper.Map<IEnumerable<BranchDto>>(branches);
            return ResponseHandler.Success(branchDtos, "Branches retrieved successfully");
        }

        public async Task<ResponseDto<BranchDto>> UpdateBranchAsync(int id, UpdateBranchDto dto)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return ResponseHandler.NotFound<BranchDto>($"Branch with ID {id} not found");
            }
            branch.Name = dto.Name;
            branch.Address = dto.Address;
            branch.Phone = dto.Phone;
            branch.IsActive = dto.IsActive;
            _context.Branches.Update(branch);
            await _context.SaveChangesAsync();
            var branchDto = _mapper.Map<BranchDto>(branch);
            return ResponseHandler.Success(branchDto, "Branch updated successfully");
        }
    }
}
