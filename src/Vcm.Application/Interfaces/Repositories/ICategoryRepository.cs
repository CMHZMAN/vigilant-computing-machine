using Vcm.Domain.Entities;

namespace Vcm.Application.Interfaces.Repositories;

public interface ICategoryRepository : IGenericRepository<Category>
{
    Task<bool> HasProductsAsync(int categoryId);
    Task<bool> ExistsByNameAsync(string name, int? excludeId = null);
}
