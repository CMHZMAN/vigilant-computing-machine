using Vcm.Application.DTOs;
using Vcm.Application.DTOs.Products;

namespace Vcm.Application.Interfaces.Services;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<PaginatedResult<ProductResponseDto>> GetPagedAsync(int page, int pageSize);
    Task<ProductResponseDto?> GetByIdAsync(int id);
    Task<ProductResponseDto> CreateAsync(ProductRequestDto request);
    Task<bool> UpdateAsync(int id, ProductRequestDto request);
    Task<bool> DeleteAsync(int id);
}
