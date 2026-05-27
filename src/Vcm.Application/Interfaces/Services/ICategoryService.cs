using Vcm.Application.DTOs.Categories;

namespace Vcm.Application.Interfaces.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    Task<CategoryResponseDto?> GetByIdAsync(int id);
    Task<CategoryResponseDto> CreateAsync(CategoryRequestDto request);
    Task<bool> UpdateAsync(int id, CategoryRequestDto request);
    Task<bool> DeleteAsync(int id);
}
