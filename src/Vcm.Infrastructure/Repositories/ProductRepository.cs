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

    public async Task<Product?> GetByIdWithCategoryAsync(int id)
    {
        return await DbContext.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
    }
}
