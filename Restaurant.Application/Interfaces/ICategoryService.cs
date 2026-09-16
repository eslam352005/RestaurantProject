using Restaurant.Application.DTOs.Category;
using Restaurant.Application.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant.Application.Interfaces
{
    public interface ICategoryService
    {
        public Task<ResponseDto<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
        public Task<ResponseDto<CategoryDto>> GetCategoryByIdAsync(int id);
        public Task<ResponseDto<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
        public Task<ResponseDto<CategoryDto>> UpdateCategory(int id, UpdateCategoryDto updateCategoryDto);
        public Task<ResponseDto<bool>> DeleteCategory(int id);
    }
}
