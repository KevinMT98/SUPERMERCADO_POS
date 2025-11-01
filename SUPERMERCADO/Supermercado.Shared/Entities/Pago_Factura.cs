using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermercado.Shared.Entities;

/// <summary>
/// Entidad que representa un pago de factura
/// </summary>
public class Pago_Factura
{
    /// <summary>
    /// Identificador único del pago
    /// </summary>
    [Key]
    public int pago_id { get; set; }

    /// <summary>
    /// Clave foránea a la factura
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_factura_id { get; set; }

    /// <summary>
    /// Clave foránea al método de pago
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_id_metodo_pago { get; set; }

    /// <summary>
    /// Monto del pago
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El {0} debe ser mayor a 0")]
    public decimal monto { get; set; }

    /// <summary>
    /// Referencia del pago (número de transacción, cheque, etc.)
    /// </summary>
    [StringLength(100, ErrorMessage = "La {0} no puede exceder {1} caracteres")]
    public string? referencia_pago { get; set; }

    // ===== PROPIEDADES DE NAVEGACIÓN =====

    /// <summary>
    /// Navegación a la factura
    /// </summary>
    [ForeignKey(nameof(FK_factura_id))]
    public Factura? Factura { get; set; }

    /// <summary>
    /// Navegación al método de pago
    /// </summary>
    [ForeignKey(nameof(FK_id_metodo_pago))]
    public Metodos_Pago? MetodoPago { get; set; }
}
