using Catalog.Core.Entities;
using Catalog.Core.Repositories.Base;

namespace Catalog.Core.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesAll();

        Task<int> CategoryCreate(Category item);

        Task<bool> CategoryModify(Category item);
        Task<bool> CategoryDelete(int id);
    }
}
