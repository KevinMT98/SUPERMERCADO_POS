using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supermercado.Shared.Entities;

/// <summary>
/// Entidad que representa un movimiento de inventario o documento
/// </summary>
public class Movimiento
{
    /// <summary>
    /// Identificador único del movimiento
    /// </summary>
    [Key]
    public int movimiento_id { get; set; }

    /// <summary>
    /// Clave foránea al tipo de documento
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_codigo_tipodoc { get; set; }

    /// <summary>
    /// Clave foránea al consecutivo
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_consecutivo_id { get; set; }

    /// <summary>
    /// Número del documento
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [StringLength(50, ErrorMessage = "El número de documento no puede exceder {1} caracteres")]
    public string numero_documento { get; set; } = null!;

    /// <summary>
    /// Fecha del movimiento
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public DateTime fecha { get; set; } = DateTime.Now;

    /// <summary>
    /// Clave foránea al usuario que registra el movimiento
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_usuario_id { get; set; }

    /// <summary>
    /// Clave foránea al tercero (cliente/proveedor)
    /// </summary>
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int FK_tercero_id { get; set; }

    /// <summary>
    /// Observaciones del movimiento
    /// </summary>
    [StringLength(500, ErrorMessage = "Las observaciones no pueden exceder {1} caracteres")]
    public string? observaciones { get; set; }

    // ===== PROPIEDADES DE NAVEGACIÓN =====

    /// <summary>
    /// Navegación al tipo de documento
    /// </summary>
    [ForeignKey(nameof(FK_codigo_tipodoc))]
    public TipoDcto? TipoDcto { get; set; }

    /// <summary>
    /// Navegación al consecutivo
    /// </summary>
    [ForeignKey(nameof(FK_consecutivo_id))]
    public Consecutivo? Consecutivo { get; set; }

    /// <summary>
    /// Navegación al usuario
    /// </summary>
    [ForeignKey(nameof(FK_usuario_id))]
    public Usuario? Usuario { get; set; }

    /// <summary>
    /// Navegación al tercero
    /// </summary>
    [ForeignKey(nameof(FK_tercero_id))]
    public Tercero? Tercero { get; set; }

    /// <summary>
    /// Navegación a las facturas asociadas a este movimiento
    /// </summary>
    public ICollection<Factura>? Facturas { get; set; }
}
