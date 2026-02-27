using Provider.Application.UseCases.Abstractions.UseCases;
using Provider.Infrastructure.Repositories;

namespace Provider.Application.UseCases.ProviderUseCases
{
    public class DeleteProviderUseCase : IInteractorUseCase<Guid, bool>
    {
        private readonly IMongoRepository _repository;

        public DeleteProviderUseCase(IMongoRepository repository)
            => _repository = repository;

        public async Task<bool> InteractAsync(string user, Guid id, CancellationToken cancellationToken)
        {
            return await _repository.DeleteAsync<Core.Domain.Aggregates.Providers.Provider>(id, cancellationToken);
            //return true;
        }
    }
}