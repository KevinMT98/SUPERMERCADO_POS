using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermercado.Shared.Entities;

/// <summary>
/// Entidad que representa una factura de venta
/// </summary>
public class Factura
{
    /// <summary>
    /// Identificador único de la factura
    /// </summary>
    [Key]
    public int factura_id { get; set; }

    /// <summary>
    /// Clave foránea al movimiento
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_movimiento_id { get; set; }

    /// <summary>
    /// Total bruto de la factura (antes de descuentos)
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal total_bruto { get; set; }

    /// <summary>
    /// Total de descuentos aplicados
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal total_descuentos { get; set; } = 0;

    /// <summary>
    /// Total neto de la factura (después de descuentos)
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal total_neto { get; set; }

    // ===== PROPIEDADES DE NAVEGACIÓN =====

    /// <summary>
    /// Navegación al movimiento
    /// </summary>
    [ForeignKey(nameof(FK_movimiento_id))]
    public Movimiento? Movimiento { get; set; }

    /// <summary>
    /// Navegación a los detalles de la factura
    /// </summary>
    public ICollection<Detalle_Factura>? DetallesFactura { get; set; }

    /// <summary>
    /// Navegación a los pagos de la factura
    /// </summary>
    public ICollection<Pago_Factura>? PagosFactura { get; set; }
}
