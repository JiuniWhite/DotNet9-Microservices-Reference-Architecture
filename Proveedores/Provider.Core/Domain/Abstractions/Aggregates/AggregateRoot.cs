using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Provider.Core.Domain.Abstractions.Entities;

namespace Provider.Core.Domain.Abstractions.Aggregates
{
    public abstract class AggregateRoot : Entity, IAggregateRoot
    {
        //[BsonId]
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        public DateTime DateAt { get; set; } = DateTime.Now;

        // [BsonId(IdGenerator = typeof(GuidGenerator))]
        // public Guid Id { get; set; }

    }
}

