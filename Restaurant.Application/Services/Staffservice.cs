using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.DTOs.Staff;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Services
{
    public class Staffservice(IApplicationDbcontext _context,UserManager<ApplicationUser> _userManager) : IStaffService
    {
        public async Task<ResponseDto<StaffDto>> CreateStaffAsync(CreateStaffDto staffDto)
        {
            var staffUser = new ApplicationUser
            {
                Email = staffDto.Email,
                FullName = staffDto.FullName,
                IsActive = true,
                CreatedAt = DateTime.Now,
            };

            await _userManager.CreateAsync(staffUser, staffDto.Password);
            await _userManager.AddToRoleAsync(staffUser, staffDto.Role.ToString());

            var staff = new Staff
            {
                FullName = staffDto.FullName,
                Role = staffDto.Role,
                BranchId = staffDto.BranchId,
                ApplicationUserId = staffUser.Id
            };
            await _context.Staff.AddAsync(staff);
            await _context.SaveChangesAsync();

            var response = new StaffDto
            {
                Id = staff.Id,
                BranchName =  (await _context.Staff.Include(s=>s.Branch).Where(s=>s.Id == staff.Id).Select(s=>s.Branch.Name).FirstOrDefaultAsync())!,
                FullName = staff.FullName,
                Email = staffUser.Email!,
                Role = staff.Role.ToString(),
                BranchId = staff.BranchId
            };
            return ResponseHandler.Success(response, "Staff created successfully");
        }

        public async Task<ResponseDto<bool>> DeleteStaffAsync(int id)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return ResponseHandler.NotFound<bool>("Staff not found");
            }
            var staffUser = await _userManager.FindByIdAsync(staff.ApplicationUserId);
            if (staffUser == null)
            {
                return ResponseHandler.NotFound<bool>("Staff user not found");
            }
            _context.Staff.Remove(staff);
            await _userManager.DeleteAsync(staffUser);
            await _context.SaveChangesAsync();
            return ResponseHandler.Success(true, "Staff deleted successfully");
        }

        public async Task<ResponseDto<StaffDto>> GetStaffByIdAsync(int id)
        {
           var staff = await _context.Staff
                .Include(s => s.Branch)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (staff == null)
            {
                return ResponseHandler.NotFound<StaffDto>("Staff not found");
            }
            var staffUser = await _userManager.FindByIdAsync(staff.ApplicationUserId);
            if(staffUser == null)
            {
                return ResponseHandler.NotFound<StaffDto>("Staff user not found");
            }
            var response = new StaffDto
            {
                Id = staff.Id,
                FullName = staff.FullName,
                Email = staffUser.Email!,
                Role = staff.Role.ToString(),
                BranchId = staff.BranchId,
                BranchName = staff.Branch.Name
            };
            return ResponseHandler.Success(response, "Staff retrieved successfully");
        }

        public async Task<ResponseDto<IEnumerable<StaffDto>>> GetStaffsAsync(int? branchId)
        {
           var staff = await _context.Staff
                .Include(s => s.Branch)
                .Where(s => !branchId.HasValue || s.BranchId == branchId.Value)
                .ToListAsync();

            var staffUsers = await _userManager.Users
                .Where(u => staff.Select(s => s.ApplicationUserId).Contains(u.Id))
                .ToListAsync();

            var response = staff.Select(s =>
            {
                var staffUser = staffUsers.FirstOrDefault(u => u.Id == s.ApplicationUserId);
                return new StaffDto
                {
                    Id = s.Id,
                    FullName = s.FullName,
                    Email = staffUser?.Email ?? string.Empty,
                    Role = s.Role.ToString(),
                    BranchId = s.BranchId,
                    BranchName = s.Branch.Name
                };
            }).ToList();

            return ResponseHandler.Success<IEnumerable<StaffDto>>(response, "Staffs retrieved successfully");
        }

        public async Task<ResponseDto<StaffDto>> UpdateStaffAsync(int id, UpdateStaffDto staffDto)
        {
            var staff = await _context.Staff.FindAsync(id);
            if (staff == null)
            {
                return ResponseHandler.NotFound<StaffDto>("Staff not found");
            }
            var staffUser = await _userManager.FindByIdAsync(staff.ApplicationUserId);
            if (staffUser == null)
            {
                return ResponseHandler.NotFound<StaffDto>("Staff user not found");
            }
            var userRoles = await _userManager.GetRolesAsync(staffUser);

            staff.FullName = staffDto.FullName;
            staff.Role = staffDto.Role;
            staff.BranchId = staffDto.BranchId;

            await _userManager.RemoveFromRolesAsync(staffUser, userRoles);
            await _userManager.AddToRoleAsync(staffUser, staffDto.Role.ToString());

            _context.Staff.Update(staff);
            await _context.SaveChangesAsync();
            return ResponseHandler.Success(new StaffDto
            {
                Id = staff.Id,
                FullName = staff.FullName,
                Email = staffUser.Email!,
                Role = staff.Role.ToString(),
                BranchId = staff.BranchId,
                BranchName = (await _context.Branches.FindAsync(staff.BranchId))?.Name ?? string.Empty
            }, "Staff updated successfully");
        }
    }
}
