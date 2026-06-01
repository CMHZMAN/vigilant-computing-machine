using Microsoft.EntityFrameworkCore;
using Vcm.Application.Interfaces.Repositories;
using Vcm.Domain.Entities;
using Vcm.Infrastructure.Data;

namespace Vcm.Infrastructure.Repositories;

public class ProductRepository(AppDbContext dbContext) : GenericRepository<Product>(dbContext), IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllWithCategoryAsync()
    {
        return await DbContext.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedWithCategoryAsync(int page, int pageSize)
    {
        var totalCount = await DbContext.Products.CountAsync();
        var items = await DbContext.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        return (items, totalCount);
    }

    public async Task<Product?> GetByIdWithCategoryAsync(int id)
    {
        return await DbContext.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null)
        => await DbContext.Products.AnyAsync(p => p.Name == name && (excludeId == null || p.Id != excludeId));
}
