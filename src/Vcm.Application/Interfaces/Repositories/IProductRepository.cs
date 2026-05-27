using Vcm.Domain.Entities;

namespace Vcm.Application.Interfaces.Repositories;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithCategoryAsync();
    Task<Product?> GetByIdWithCategoryAsync(int id);
}
