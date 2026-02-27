using Provider.Application.UseCases.Abstractions.UseCases;
using Provider.Core.Models;
using Provider.Infrastructure.Repositories;

namespace Provider.Application.UseCases.ProviderUseCases
{
    public class UpdateProviderUseCase : IInteractorUseCase<ProviderModel.UpdateProvider, bool>
    {
        private readonly IMongoRepository _repository;

        public UpdateProviderUseCase(IMongoRepository repository)
            => _repository = repository;

        public async Task<bool> InteractAsync(string user, ProviderModel.UpdateProvider request, CancellationToken cancellationToken)
        {
            if (request.Item.Id == null) return false;

            //if (user == null) return false;

            var obj = new Provider.Core.Domain.Aggregates.Providers.Provider()
            {
                Id = request.Item.Id.Value,
                Name = request.Item.Name,
                ServiceIds = request.Item.ServiceIds,
                Contact = new Core.Domain.Aggregates.Providers.ContactInfo()
                {
                    Email = request.Item.Contact.Email,
                    Phone = request.Item.Contact.Phone
                }
            };



            // En Mongo, solemos buscar por Id y reemplazar o actualizar campos específicos
            await _repository.UpdateAsync(obj.Id, obj);
            return true;
        }
    }
}