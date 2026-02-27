using System;
using System.Collections.Generic;

namespace Provider.Core.Models
{
    public class ProviderModel
    {
        // 2. Usamos Positional Records: Más cortos, inmutables y limpios
        public record ProviderData(
            Guid? Id,
            string Name,
            List<string> ServiceIds,
            ContactInfo Contact
        );

        public record ContactInfo(
            string Email,
            string Phone
        );

        // 3. Definición de Comandos/Queries
        public record CreateProvider(ProviderData Item);
        public record UpdateProvider(ProviderData Item);
        public record DeleteProvider(Guid Id);

        // 4. CORRECCIÓN: Si es virtual, la clase NO debe ser static.
        // Usamos IReadOnlyCollection para proteger la integridad de los datos
        public virtual IEnumerable<ProviderData> Items { get; set; } = new List<ProviderData>();
    }
}