using Vcm.Application.DTOs.Categories;
using Vcm.Application.Interfaces.Repositories;
using Vcm.Application.Interfaces.Services;
using Vcm.Domain.Entities;

namespace Vcm.Application.Services;

public class CategoryService(ICategoryRepository categoryRepository) : ICategoryService
{
    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        return categories.Select(MapToResponse);
    }

    public async Task<CategoryResponseDto?> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);
        return category is null ? null : MapToResponse(category);
    }

    public async Task<CategoryResponseDto> CreateAsync(CategoryRequestDto request)
    {
        Validate(request);

        var category = new Category
        {
            Name = request.Name.Trim()
        };

        var created = await categoryRepository.AddAsync(category);
        return MapToResponse(created);
    }

    public async Task<bool> UpdateAsync(int id, CategoryRequestDto request)
    {
        Validate(request);

        var existing = await categoryRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        existing.Name = request.Name.Trim();
        await categoryRepository.UpdateAsync(existing);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await categoryRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        await categoryRepository.DeleteAsync(existing);
        return true;
    }

    private static CategoryResponseDto MapToResponse(Category category) => new()
    {
        Id = category.Id,
        Name = category.Name
    };

    private static void Validate(CategoryRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Category name is required.");
        }
    }
}
