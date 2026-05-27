using Vcm.Application.Interfaces.Repositories;
using Vcm.Domain.Entities;
using Vcm.Infrastructure.Data;

namespace Vcm.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext dbContext) : GenericRepository<Category>(dbContext), ICategoryRepository
{
}
