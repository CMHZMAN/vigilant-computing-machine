using Vcm.Domain.Entities;

namespace Vcm.Application.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithCategoryAsync();
    Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedWithCategoryAsync(int page, int pageSize);
    Task<Product?> GetByIdWithCategoryAsync(int id);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
}
