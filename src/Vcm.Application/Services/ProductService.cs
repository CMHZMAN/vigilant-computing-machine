using Vcm.Application.DTOs;
using Vcm.Application.DTOs.Products;
using Vcm.Application.Interfaces.Repositories;
using Vcm.Application.Interfaces.Services;
using Vcm.Domain.Entities;

namespace Vcm.Application.Services;

public class ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository) : IProductService
{
    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = await productRepository.GetAllWithCategoryAsync();
        return products.Select(MapToResponse);
    }

    public async Task<ProductResponseDto?> GetByIdAsync(int id)
    {
        var product = await productRepository.GetByIdWithCategoryAsync(id);
        return product is null ? null : MapToResponse(product);
    }

    public async Task<PaginatedResult<ProductResponseDto>> GetPagedAsync(int page, int pageSize)
    {
        var (items, totalCount) = await productRepository.GetPagedWithCategoryAsync(page, pageSize);
        return new PaginatedResult<ProductResponseDto>
        {
            Items = items.Select(MapToResponse),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ProductResponseDto> CreateAsync(ProductRequestDto request)
    {
        await ValidateAsync(request);

        var product = new Product
        {
            Name = request.Name.Trim(),
            Price = request.Price,
            CategoryId = request.CategoryId
        };

        var created = await productRepository.AddAsync(product);
        var createdWithCategory = await productRepository.GetByIdWithCategoryAsync(created.Id) ?? created;

        return MapToResponse(createdWithCategory);
    }

    public async Task<bool> UpdateAsync(int id, ProductRequestDto request)
    {
        await ValidateAsync(request, excludeId: id);

        var existing = await productRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        existing.Name = request.Name.Trim();
        existing.Price = request.Price;
        existing.CategoryId = request.CategoryId;

        await productRepository.UpdateAsync(existing);
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await productRepository.GetByIdAsync(id);
        if (existing is null)
        {
            return false;
        }

        await productRepository.DeleteAsync(existing);
        return true;
    }

    private static ProductResponseDto MapToResponse(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Price = product.Price,
        CategoryId = product.CategoryId,
        CategoryName = product.Category?.Name ?? string.Empty
    };

    private async Task ValidateAsync(ProductRequestDto request, int? excludeId = null)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Product name is required.");

        if (request.Price <= 0)
            throw new ArgumentException("Product price must be greater than zero.");

        if (await productRepository.ExistsByNameAsync(request.Name.Trim(), excludeId))
            throw new ArgumentException($"A product named '{request.Name.Trim()}' already exists.");

        if (!await categoryRepository.ExistsAsync(request.CategoryId))
            throw new ArgumentException("Category does not exist.");
    }
}
