using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.MenuItem;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Services
{
    public class MenuitemService(IApplicationDbcontext _context,IMapper _mapper ,IWebHostEnvironment _env) : IMenuItemService
    {
        public async Task<ResponseDto<MenuItemDto>> CreateMenuItemAsync(CreateMenuItemDto dto)
        {
            var item = _mapper.Map<MenuItem>(dto);
           await _context.MenuItems.AddAsync(item);
           await _context.SaveChangesAsync();
           var itemDto = _mapper.Map<MenuItemDto>(item);
           return ResponseHandler.Success(itemDto, "Menu item created successfully");
        }

        public async Task<ResponseDto<bool>> DeleteMenuItem(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null)
            {
                return ResponseHandler.NotFound<bool>($"Menu item with ID {id} not found");
            }
            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return ResponseHandler.Success(true, "Menu item deleted successfully");
        }

        public async Task<ResponseDto<MenuItemDto>> GetMenuItemByIdAsync(int id)
        {
            var item = await _context.MenuItems.Include(i => i.Category).FirstOrDefaultAsync(i => i.Id == id);
            if (item == null)
            {
                return ResponseHandler.NotFound<MenuItemDto>($"Menu item with ID {id} not found");
            }
            var itemDto = _mapper.Map<MenuItemDto>(item);
            return ResponseHandler.Success(itemDto, "Menu item retrieved successfully");
        }

        public async Task<ResponseDto<IEnumerable<MenuItemDto>>> GetMenuItemsAsync(int? branchId, int? categoryId)
        {
            var Items = await _context.MenuItems
                .Include(i => i.Category)
                .Where(i =>  (branchId == null || i.BranchId == branchId) && (categoryId == null || i.CategoryId == categoryId))
                .ToListAsync();
            
            var ItemsDto = _mapper.Map<IEnumerable<MenuItemDto>>(Items);
            return ResponseHandler.Success(ItemsDto, "Menu items retrieved successfully");
        }

        public async Task<ResponseDto<MenuItemDto>> UpdateMenuItem(int id, UpdateMenuItemDto dto)
        {
           var item = await _context.MenuItems.FindAsync(id);
            if (item == null)
            {
                return ResponseHandler.NotFound<MenuItemDto>($"Menu item with ID {id} not found");
            }
            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Price = dto.Price;
            item.CategoryId = dto.CategoryId;
            item.IsAvailable = dto.IsAvailable;
            _context.MenuItems.Update(item);
            await _context.SaveChangesAsync();
            var itemDto = _mapper.Map<MenuItemDto>(item);
            return ResponseHandler.Success(itemDto, "Menu item updated successfully");
        }

        public async Task<ResponseDto<MenuItemDto>> UpdateMenuItemAvailability(int id, UpdateMenuItemAvailabilityDto dto)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null)
            {
                return ResponseHandler.NotFound<MenuItemDto>($"Menu item with ID {id} not found");
            }
            item.IsAvailable = dto.IsAvailable;
            _context.MenuItems.Update(item);
            await _context.SaveChangesAsync();
            var itemDto = _mapper.Map<MenuItemDto>(item);
            return ResponseHandler.Success(itemDto, "Menu item availability updated successfully");
        }

        public async Task<ResponseDto<string>> UploadImageAsync(int id, IFormFile file)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem is null)
                return ResponseHandler.NotFound<string>("MenuItem not found");

            if (file is null || file.Length == 0)
                return ResponseHandler.BadRequest<string>("No file uploaded");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(ext))
                return ResponseHandler.BadRequest<string>("Invalid file type");

            var fileName = $"{Guid.NewGuid()}{ext}";
            var folderPath = Path.Combine(_env.WebRootPath, "Images", "MenuItems");
            Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // امسح الصورة القديمة لو موجودة
            if (!string.IsNullOrEmpty(menuItem.ImageUrl))
            {
                var oldPath = Path.Combine(_env.WebRootPath, menuItem.ImageUrl.TrimStart('/'));
                if (File.Exists(oldPath)) File.Delete(oldPath);
            }

            menuItem.ImageUrl = $"/Images/MenuItems/{fileName}";
            await _context.SaveChangesAsync(default);

            return ResponseHandler.Success(menuItem.ImageUrl, "Image uploaded successfully");
        }
    }
}
