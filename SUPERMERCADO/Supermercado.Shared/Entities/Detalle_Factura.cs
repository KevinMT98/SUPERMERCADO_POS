using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermercado.Shared.Entities;

/// <summary>
/// Entidad que representa el detalle de una factura (línea de productos)
/// </summary>
public class Detalle_Factura
{
    /// <summary>
    /// Identificador único del detalle de factura
    /// </summary>
    [Key]
    public int detalle_id { get; set; }

    /// <summary>
    /// Clave foránea a la factura
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_factura_id { get; set; }

    /// <summary>
    /// Clave foránea al producto
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_producto_id { get; set; }

    /// <summary>
    /// Cantidad del producto
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "La {0} debe ser mayor a 0")]
    public int cantidad { get; set; }

    /// <summary>
    /// Precio unitario del producto
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal precio_unitario { get; set; }

    /// <summary>
    /// Porcentaje de descuento aplicado
    /// </summary>
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100, ErrorMessage = "El {0} debe estar entre 0 y 100")]
    public decimal descuento_porcentaje { get; set; } = 0;

    /// <summary>
    /// Valor del descuento aplicado
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal descuento_valor { get; set; } = 0;

    /// <summary>
    /// Subtotal de la línea (cantidad * precio_unitario - descuento_valor)
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal subtotal { get; set; }

    // ===== PROPIEDADES DE NAVEGACIÓN =====

    /// <summary>
    /// Navegación a la factura
    /// </summary>
    [ForeignKey(nameof(FK_factura_id))]
    public Factura? Factura { get; set; }

    /// <summary>
    /// Navegación al producto
    /// </summary>
    [ForeignKey(nameof(FK_producto_id))]
    public Producto? Producto { get; set; }
}
