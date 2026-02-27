using System.Linq.Expressions;
using Provider.Core.Domain.Abstractions.Aggregates;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Provider.Infrastructure.Data;

namespace Provider.Infrastructure.Repositories
{
    public class MongoRepository : IMongoRepository
    {
        private readonly IMongoContext _context;

        public MongoRepository(IMongoContext context)
            => _context = context;

      

        public async Task<Guid> InsertAsync<TCollection>(TCollection collection, CancellationToken cancellationToken)
            where TCollection : class, IAggregateRoot
        {
            await _context.Collection<TCollection>().InsertOneAsync(collection, default, cancellationToken);
            return collection.Id;
        }

        public async Task<Guid> UpdateAsync<TCollection>(Guid id, TCollection collection)
             where TCollection : class, IAggregateRoot
        {

            await _context.Collection<TCollection>().ReplaceOneAsync(x => x.Id == id, collection);
            return collection.Id;
        }

        public Task<TCollection> FindAsync<TCollection>(Expression<Func<TCollection, bool>> predicate, CancellationToken cancellationToken)
            where TCollection : class, IAggregateRoot
            => _context.Collection<TCollection>().AsQueryable().Where(predicate).FirstOrDefaultAsync(cancellationToken);
        
 
        //public Task<List<TCollection>> ListAsync<TCollection>(CancellationToken cancellationToken)
        //    where TCollection : class, IAggregateRoot
        //    => _context.Collection<TCollection>().AsQueryable().ToListAsync(cancellationToken);

        public Task<List<TCollection>> ListAsync<TCollection>(Expression<Func<TCollection, bool>> predicate, CancellationToken cancellationToken)
            where TCollection : class, IAggregateRoot
            => _context.Collection<TCollection>().AsQueryable().Where(predicate).ToListAsync(cancellationToken);

      
        public async Task<bool> DeleteAsync<TCollection>(Guid id, CancellationToken cancellationToken)
            where TCollection : class, IAggregateRoot
        {
            await _context.Collection<TCollection>().DeleteOneAsync(x => x.Id == id, cancellationToken);
            return true;
        }

        public IMongoQueryable<TCollection> QryAsync<TCollection>(Expression<Func<TCollection, bool>> predicate)
            where TCollection : class, IAggregateRoot
        {
            return _context.Collection<TCollection>().AsQueryable().Where(predicate);
        }

        
    }
}

