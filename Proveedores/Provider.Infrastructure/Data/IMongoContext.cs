using MongoDB.Driver;
using Provider.Core.Domain.Abstractions.Aggregates;

namespace Provider.Infrastructure.Data
{
    public interface IMongoContext
    {
        IMongoCollection<TCollection> Collection<TCollection>()
            where TCollection : class, IAggregateRoot;
    }
}

