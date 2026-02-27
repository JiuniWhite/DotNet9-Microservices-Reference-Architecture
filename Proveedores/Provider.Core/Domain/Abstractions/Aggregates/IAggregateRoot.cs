namespace Provider.Core.Domain.Abstractions.Aggregates
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}

