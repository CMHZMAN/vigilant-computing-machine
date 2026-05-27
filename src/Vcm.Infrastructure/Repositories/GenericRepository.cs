using Microsoft.EntityFrameworkCore;
using Vcm.Application.Interfaces.Repositories;
using Vcm.Infrastructure.Data;

namespace Vcm.Infrastructure.Repositories;

public class GenericRepository<T>(AppDbContext dbContext) : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext DbContext = dbContext;
    protected readonly DbSet<T> DbSet = dbContext.Set<T>();

    public virtual async Task<IEnumerable<T>> GetAllAsync() => await DbSet.AsNoTracking().ToListAsync();

    public virtual async Task<T?> GetByIdAsync(int id) => await DbSet.FindAsync(id);

    public virtual async Task<T> AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
        await DbContext.SaveChangesAsync();
        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(T entity)
    {
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(int id) => await DbSet.FindAsync(id) is not null;
}
