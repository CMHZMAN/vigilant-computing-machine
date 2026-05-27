using Vcm.Frontend.Models;

namespace Vcm.Frontend.Services;

public interface IApiClientService
{
    Task<List<CategoryResponseDto>> GetCategoriesAsync();
    Task<CategoryResponseDto> CreateCategoryAsync(CategoryRequestDto request);
    Task UpdateCategoryAsync(int id, CategoryRequestDto request);
    Task DeleteCategoryAsync(int id);

    Task<List<ProductResponseDto>> GetProductsAsync();
    Task<ProductResponseDto> CreateProductAsync(ProductRequestDto request);
    Task UpdateProductAsync(int id, ProductRequestDto request);
    Task DeleteProductAsync(int id);
}
