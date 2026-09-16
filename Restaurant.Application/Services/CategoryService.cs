using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Restaurant.Application.DTOs.Category;
using Restaurant.Application.DTOs.Responses;
using Restaurant.Application.Interfaces;
using Restaurant.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Services
{
    public class CategoryService(IApplicationDbcontext _context,IMapper _mapper) : ICategoryService
    {
        public async Task<ResponseDto<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var category = _mapper.Map<Category>(createCategoryDto);
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return ResponseHandler.Success(categoryDto, "Category created successfully");
        }

        public async Task<ResponseDto<bool>> DeleteCategory(int id)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null)
            {
                return ResponseHandler.NotFound<bool>($"Category with ID {id} not found");
            }
            _context.Categories.Remove(cat);
            await _context.SaveChangesAsync();
            return ResponseHandler.Success(true, "Category deleted successfully");
        }

        public async Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            var cats = await _context.Categories.ToListAsync();
            var catsDto = _mapper.Map<IEnumerable<CategoryDto>>(cats);
            return ResponseHandler.Success(catsDto, "Categories retrieved successfully");
        }

        public async Task<ResponseDto<CategoryDto>> GetCategoryByIdAsync(int id)
        {
           var cat = await _context.Categories.FindAsync(id);
            if (cat == null)
            {
                return ResponseHandler.NotFound<CategoryDto>($"Category with ID {id} not found");
            }
            var catDto = _mapper.Map<CategoryDto>(cat);
            return ResponseHandler.Success(catDto, "Category retrieved successfully");
        }

        public async Task<ResponseDto<CategoryDto>> UpdateCategory(int id, UpdateCategoryDto updateCategoryDto)
        {
            var cat = await _context.Categories.FindAsync(id);
            if (cat == null)
            {
                return ResponseHandler.NotFound<CategoryDto>($"Category with ID {id} not found");
            }
            cat.Name = updateCategoryDto.Name;
            _context.Categories.Update(cat);
            await _context.SaveChangesAsync();
            var catDto = _mapper.Map<CategoryDto>(cat);
            return ResponseHandler.Success(catDto, "Category updated successfully");
        }
    }
}
