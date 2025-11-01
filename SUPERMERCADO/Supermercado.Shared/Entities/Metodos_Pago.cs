using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.Entities;

/// <summary>
/// Entidad que representa los métodos de pago disponibles
/// </summary>
public class Metodos_Pago
{
    /// <summary>
    /// Identificador único del método de pago
    /// </summary>
    [Key]
    public int id_metodo_pago { get; set; }

    /// <summary>
    /// Nombre del método de pago (Efectivo, Tarjeta, Transferencia, etc.)
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(100, ErrorMessage = "El {0} no puede exceder {1} caracteres")]
    public string metodo_pago { get; set; } = null!;

    /// <summary>
    /// Código o abreviatura del método de pago
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(20, ErrorMessage = "El {0} no puede exceder {1} caracteres")]
    public string codigo_metpag { get; set; } = null!;

    /// <summary>
    /// Indica si el método de pago está activo
    /// </summary>
    public bool activo { get; set; } = true;

    // ===== PROPIEDADES DE NAVEGACIÓN =====

    /// <summary>
    /// Navegación a los pagos de factura que usan este método
    /// </summary>
    public ICollection<Pago_Factura>? PagosFactura { get; set; }
}
