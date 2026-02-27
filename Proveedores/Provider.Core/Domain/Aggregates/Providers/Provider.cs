using Provider.Core.Domain.Abstractions.Aggregates;

namespace Provider.Core.Domain.Aggregates.Providers
{
    public class Provider : AggregateRoot, IProvider
    {

        public string Name { get; set; } = null!;

        // Lista de IDs de servicios que viven en el Microservicio de Catálogo
        public List<string> ServiceIds { get; set; } = new();

        public ContactInfo Contact { get; set; } = null!;

        protected override bool Validate() => OnValidate<ProviderValidator, Provider>();
    }

    public class ContactInfo
    {
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
    }
}
