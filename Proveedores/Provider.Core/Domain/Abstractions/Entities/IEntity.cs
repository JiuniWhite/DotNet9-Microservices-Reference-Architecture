using FluentValidation.Results;

namespace Provider.Core.Domain.Abstractions.Entities
{
    public interface IEntity
    {
        bool IsDeleted { get; }

        bool IsValid { get; }

        public IEnumerable<ValidationFailure> Errors { get; }
    }
}

