using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Core.Entities.Base;

namespace Catalog.Core.Entities;

[Table("Services", Schema = "Dbo")]
public partial class Service : Entity
{
    //public Guid Id { get; set; }
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres.")]
    public string Name { get; set; }

    [StringLength(800, ErrorMessage = "La descripción no puede exceder 800 caracteres.")]
    public string Description { get; set; }
    public decimal BasePrice { get; set; }
    public string Benefits { get; set; } // Aquí van los beneficios detallados
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
