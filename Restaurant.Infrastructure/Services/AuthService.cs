using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Restaurant.Application.DTOs.Auth;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using Restaurant.Domain.Enums;
using Restaurant.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Restaurant.Infrastructure.Services
{
    public class AuthService(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager,ApplicationDbcontext _dbcontext,IConfiguration _configuration) : IAuthService
    {
       
        public async Task<ResponseDto<bool>> AssignRoleAsync(string id, StaffRole role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return ResponseHandler.NotFound<bool>("User not found.");
            }
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.AddToRoleAsync(user, role.ToString());
            return ResponseHandler.Success<bool>(true, "Role assigned successfully.");
        }

        public async Task<ResponseDto<UserResponseDto>> LoginAsync(LoginDto loginDto)
        {
           var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                return ResponseHandler.NotFound<UserResponseDto>("User not found.");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
            {
                return ResponseHandler.BadRequest<UserResponseDto>("Invalid credentials.");
            }
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            var staff = _dbcontext.Set<Staff>().Include(s => s.Branch).FirstOrDefault(s => s.ApplicationUserId == user.Id);
            if (staff == null)
            {
                return ResponseHandler.BadRequest<UserResponseDto>("Failed to retrieve staff information.");
            }
            return ResponseHandler.Success<UserResponseDto>(new UserResponseDto
            {
                StaffId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber!,
                Role = await _userManager.GetRolesAsync(user) != null ? (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? "Waiter" : "Waiter",
                BranchId = staff.BranchId,
                BranchName = staff.Branch.Name,
                AccessToken = await CreateTokenAsync(user),
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.Now.AddDays(7)
            }, "User logged in successfully.");
        }

        public async Task<ResponseDto<bool>> LogoutAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user == null)
            {
                return ResponseHandler.NotFound<bool>("User not found.");
            }
            
            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;
            await _userManager.UpdateAsync(user);
            return ResponseHandler.Success<bool>(true, "User logged out successfully.");

        }

        public async Task<ResponseDto<UserResponseDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
           var user = await _userManager.Users.FirstOrDefaultAsync(u=>u.RefreshToken == refreshTokenDto.RefreshToken && u.RefreshTokenExpiryTime > DateTime.Now);
            if (user == null)
            {
                return ResponseHandler.NotFound<UserResponseDto>("Invalid refresh token.");
            }
            
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
            await _userManager.UpdateAsync(user);
            var staff = _dbcontext.Set<Staff>().Include(s => s.Branch).FirstOrDefault(s => s.ApplicationUserId == user.Id);
            if (staff == null)
            {
                return ResponseHandler.BadRequest<UserResponseDto>("Failed to retrieve staff information.");
            }
            return ResponseHandler.Success<UserResponseDto>(new UserResponseDto
            {
                StaffId = user.Id,
                Email = user.Email!,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber!,
                Role = _userManager.GetRolesAsync(user).Result != null ? (_userManager.GetRolesAsync(user).Result).FirstOrDefault() ?? "Waiter" : "Waiter",
                BranchId = staff.BranchId,
                BranchName = staff.Branch.Name,
                AccessToken = await CreateTokenAsync(user),
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.Now.AddDays(7)
            }, "Token refreshed successfully.");
        }

        public async Task<ResponseDto<UserResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if(user != null)
            {
                return ResponseHandler.BadRequest<UserResponseDto>("User already exists.");
            }
            var newUser = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FullName = registerDto.FullName,
                PhoneNumber = registerDto.PhoneNumber,
                IsActive = true,
                CreatedAt = DateTime.Now
            };
            await _userManager.CreateAsync(newUser, registerDto.Password);
            await _userManager.AddToRoleAsync(newUser, "Waiter");
            var createdStaff = await _dbcontext.Set<Staff>().AddAsync(new Staff
            {
                ApplicationUserId = newUser.Id,
                BranchId = registerDto.BranchId,
                FullName=registerDto.FullName,
                Role = StaffRole.Waiter
            });
            await _dbcontext.SaveChangesAsync();
            var Staff = await _dbcontext.Set<Staff>().Include(s => s.Branch).FirstOrDefaultAsync(s => s.ApplicationUserId == newUser.Id);

            return ResponseHandler.Success<UserResponseDto>(new UserResponseDto
            {
                StaffId = newUser.Id,
                Email = newUser.Email!,
                FullName = newUser.FullName,
                PhoneNumber = newUser.PhoneNumber!,
                Role = "Waiter",
                BranchId = registerDto.BranchId,
                BranchName = Staff!.Branch.Name,
                AccessToken = await CreateTokenAsync(newUser),
                RefreshToken = GenerateRefreshToken(),
                ExpiresAt = DateTime.Now.AddDays(7)
            }, "User registered successfully.");
        }









        // Helper Methods:

        private async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
    {
        new Claim(ClaimTypes.Name, user.UserName!),
        new Claim(ClaimTypes.Email, user.Email!),
        new Claim(ClaimTypes.NameIdentifier, user.Id!),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // ✅ جيب بيانات الـ Staff المرتبطة باليوزر ده
            var staff = await _dbcontext.Staff
                .FirstOrDefaultAsync(s => s.ApplicationUserId == user.Id);

            if (staff != null)
            {
                claims.Add(new Claim("StaffId", staff.Id.ToString()));
                claims.Add(new Claim("BranchId", staff.BranchId.ToString()));
            }

            var secretKey = _configuration["JwtOptions:SecretKey"];

            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT Secret Key is not configured");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
