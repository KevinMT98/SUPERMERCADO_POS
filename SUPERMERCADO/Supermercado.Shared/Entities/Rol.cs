using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.Entities;

public class Rol
{
    [Key]
    public int rol_id { get; set; }

    [MaxLength(100, ErrorMessage ="El campo {0} no puede tener mas de {1} carácteres")]
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public string nombre { get; set; } = null!;
}
