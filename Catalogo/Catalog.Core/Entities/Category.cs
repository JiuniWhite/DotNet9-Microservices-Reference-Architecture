using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Catalog.Core.Entities.Base;

namespace Catalog.Core.Entities;


[Table("Categories", Schema = "Dbo")]
public partial class Category : Entity
{
    //public int Id { get; set; }
    [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres.")]
    public string Name { get; set; }

    [StringLength(800, ErrorMessage = "La descripción no puede exceder 800 caracteres.")]
    public string Description { get; set; }
}
