using Restaurant.Application.DTOs.Auth;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface IAuthService
    {
        public Task<ResponseDto<UserResponseDto>> RegisterAsync(RegisterDto registerDto);
        public Task<ResponseDto<UserResponseDto>> LoginAsync(LoginDto loginDto);
        public Task<ResponseDto<bool>> AssignRoleAsync(string id, StaffRole role);
        public Task<ResponseDto<UserResponseDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        public Task<ResponseDto<bool>> LogoutAsync(string id);
    }
}
