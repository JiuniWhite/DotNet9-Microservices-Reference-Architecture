using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;
public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(CatalogContext dbContext) : base(dbContext)
    {
    }

    public async Task<int> CategoryCreate(Category item)
    {
        await _dbContext.Categories.AddAsync(item);
        await _dbContext.SaveChangesAsync();
        return item.Id;
    }

    public Task<bool> CategoryDelete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CategoryModify(Category item)
    {
        var category = await _dbContext.Categories.FindAsync(item.Id);
        if (category == null)
            return false;

        category.Name = item.Name;
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Category>> GetCategoriesAll()
    {
        return await _dbContext.Categories.AsNoTracking().ToListAsync();
    }
}
