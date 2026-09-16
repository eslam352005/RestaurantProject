using Microsoft.AspNetCore.Http;
using Restaurant.Application.DTOs.MenuItem;
using Restaurant.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface IMenuItemService
    {
        public Task<ResponseDto<IEnumerable<MenuItemDto>>> GetMenuItemsAsync(int? branchId,int? categoryId);
        public Task<ResponseDto<MenuItemDto>> CreateMenuItemAsync(CreateMenuItemDto dto);
        public Task<ResponseDto<MenuItemDto>> GetMenuItemByIdAsync(int id);
        public Task<ResponseDto<MenuItemDto>> UpdateMenuItem(int id, UpdateMenuItemDto dto);
        public Task<ResponseDto<MenuItemDto>> UpdateMenuItemAvailability(int id, UpdateMenuItemAvailabilityDto dto);
        public Task<ResponseDto<bool>> DeleteMenuItem(int id);
        public Task<ResponseDto<string>> UploadImageAsync(int id, IFormFile file); 
    }
}
