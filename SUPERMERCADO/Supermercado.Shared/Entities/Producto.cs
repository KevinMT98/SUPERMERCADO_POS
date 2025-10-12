using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermercado.Shared.Entities;

public class Producto
{
    [Key]
    public int producto_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(50, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    public string codigo_producto { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    public string codigo_barras { get; set; } = null!;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [MaxLength(200, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    public string nombre { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres")]
    public string? descripcion { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal precio_unitario { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_categoria_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_codigo_iva { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int stock_actual { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int stock_minimo { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int stock_maximo { get; set; }

    public bool activo { get; set; } = true;

    // Propiedades de navegación
    [ForeignKey(nameof(FK_categoria_id))]
    public Categoria_Producto? Categoria { get; set; }

    [ForeignKey(nameof(FK_codigo_iva))]
    public Tarifa_IVA? TarifaIVA { get; set; }
}
