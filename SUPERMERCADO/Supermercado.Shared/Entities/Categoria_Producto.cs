using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.Entities;

public class Categoria_Producto
{
    [Key]
    public int categoriaId { get; set; }

    [MaxLength(250, ErrorMessage = "El campo {0} no puede tener mas de {1}")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string descripcion { get; set; } = null!;
}
