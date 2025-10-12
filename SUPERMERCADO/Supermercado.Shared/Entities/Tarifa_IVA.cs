using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Supermercado.Shared.Entities;

public class Tarifa_IVA
{
    [Key]
    public int tarifa_iva_id { get; set; }
    [Required]
    [MaxLength(10, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]
    public string codigo_Iva { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener mas de {1} carácteres")]
    [Required(ErrorMessage =" El campo {0} es obligatorio")]
    public string descripcion { get; set; } = null!;

    public decimal porcentaje { get; set; }
    public bool estado { get; set; }
}
