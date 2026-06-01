using Microsoft.EntityFrameworkCore;
using Vcm.Application.Interfaces.Repositories;
using Vcm.Domain.Entities;
using Vcm.Infrastructure.Data;

namespace Vcm.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext dbContext) : GenericRepository<Category>(dbContext), ICategoryRepository
{
    public override async Task<bool> ExistsAsync(int id)
        => await DbContext.Categories.AnyAsync(c => c.Id == id);

    public async Task<bool> HasProductsAsync(int categoryId)
        => await DbContext.Products.AnyAsync(p => p.CategoryId == categoryId);

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        => await DbContext.Categories.AnyAsync(c => c.Name == name && (excludeId == null || c.Id != excludeId));
}
