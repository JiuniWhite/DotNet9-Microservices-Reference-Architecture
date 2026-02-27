using System.Linq.Expressions;
using Catalog.Core.Entities.Base;

namespace Catalog.Core.Repositories.Base;
public interface IRepository<T> where T : Entity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null!,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
                                        string includeString = null!,
                                        bool disableTracing = true);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null!,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null!,
                                        List<Expression<Func<T, object>>> includes = null!,
                                        bool disableTracing = true);

        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }

