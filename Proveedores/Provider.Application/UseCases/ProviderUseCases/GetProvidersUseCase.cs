using Provider.Application.UseCases.Abstractions.UseCases;
using Provider.Core.Models;
using Provider.Infrastructure.Repositories;

namespace Provider.Application.UseCases.ProviderUseCases
{
    public class GetProvidersUseCase : IInteractorUseCase<object?, IEnumerable<ProviderModel.ProviderData>>
    {
        private readonly IMongoRepository _repository;

        public GetProvidersUseCase(IMongoRepository repository)
            => _repository = repository;

        public async Task<IEnumerable<ProviderModel.ProviderData>> InteractAsync(string user, object? request, CancellationToken cancellationToken)
        {
            // Obtenemos todos los documentos de la colección de Mongo
            var items = await _repository.ListAsync<Core.Domain.Aggregates.Providers.Provider>(p => true, cancellationToken);

            return items.Select(item => new ProviderModel.ProviderData(
        Id: item.Id,
        Name: item.Name ?? "Sin nombre",
        // OJO: Aquí debes pasar los campos que definimos en tu Record:
        // ServiceIds espera una List<string>
        ServiceIds: item.ServiceIds ?? new List<string>(),
        // Contact espera un record de tipo ContactInfo
        Contact: new ProviderModel.ContactInfo(
            Email: item.Contact.Email ?? string.Empty,
            Phone: item.Contact.Phone ?? string.Empty
        )
    ));
        }
    }
}