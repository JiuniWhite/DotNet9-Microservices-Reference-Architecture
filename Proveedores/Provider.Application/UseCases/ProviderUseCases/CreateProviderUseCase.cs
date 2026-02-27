namespace Provider.Application.UseCases.ProviderUseCases
{
    using Provider.Application.UseCases.Abstractions.UseCases;
    using Provider.Core.Models;
    using Provider.Infrastructure.Repositories;

    public class CreateProviderUseCase : IInteractorUseCase<ProviderModel.CreateProvider, bool>
    {
        private readonly IMongoRepository _repository;

        public CreateProviderUseCase(IMongoRepository repository)
            => _repository = repository;

        public async Task<bool> InteractAsync(string user, ProviderModel.CreateProvider request, CancellationToken cancellationToken)
        {
            // Mapeo manual del Record al Agregado/Entidad de Dominio
            // Aquí podrías validar si el nombre ya existe antes de insertar
            var m = new Provider.Core.Domain.Aggregates.Providers.Provider()
            {
               // Id = request.Item.Id ?? Guid.NewGuid(),
                Name = request.Item.Name,
                ServiceIds = request.Item.ServiceIds,
                Contact = new Core.Domain.Aggregates.Providers.ContactInfo()
                {
                    Email = request.Item.Contact.Email,
                    Phone = request.Item.Contact.Phone
                }
            };

            await _repository.InsertAsync(m, cancellationToken);
            return true;
        }
    }
}