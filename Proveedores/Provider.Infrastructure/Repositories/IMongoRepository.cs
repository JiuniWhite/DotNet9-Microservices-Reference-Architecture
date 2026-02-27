using System.Linq.Expressions;
using MongoDB.Driver.Linq;
using Provider.Core.Domain.Abstractions.Aggregates;

namespace Provider.Infrastructure.Repositories
{
    public interface IMongoRepository
    {
     //   ValueTask<IMongoCollection<T>> Get<T>(CancellationToken ct) where T : class, new();
        
        Task<Guid> InsertAsync<TCollection>(TCollection collection, CancellationToken cancellationToken)
            where TCollection : class, IAggregateRoot;

        Task<TCollection> FindAsync<TCollection>(Expression<Func<TCollection, bool>> predicate, CancellationToken cancellationToken)
            where TCollection : class, IAggregateRoot;



        Task<List<TCollection>> ListAsync<TCollection>(Expression<Func<TCollection, bool>> predicate, CancellationToken cancellationToken)
            where TCollection : class, IAggregateRoot;


        IMongoQueryable<TCollection> QryAsync<TCollection>(Expression<Func<TCollection, bool>> predicate)
           where TCollection : class, IAggregateRoot;

        Task<Guid> UpdateAsync<TCollection>(Guid id, TCollection collection)
              where TCollection : class, IAggregateRoot;


        Task<bool> DeleteAsync<TCollection>(Guid id, CancellationToken cancellationToken)
             where TCollection : class, IAggregateRoot;


       

    }
}

