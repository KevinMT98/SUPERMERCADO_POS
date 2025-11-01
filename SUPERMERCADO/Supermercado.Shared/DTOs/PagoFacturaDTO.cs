using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.DTOs;

/// <summary>
/// DTO para lectura de pagos de factura
/// </summary>
public class PagoFacturaDTO
{
    public int pago_id { get; set; }
    public int factura_id { get; set; }
    public int id_metodo_pago { get; set; }
    public decimal monto { get; set; }
    public string? referencia_pago { get; set; }
}

/// <summary>
/// DTO para creación de pagos de factura
/// </summary>
public class PagoFacturaCreateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int factura_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int id_metodo_pago { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El {0} debe ser mayor a 0")]
    public decimal monto { get; set; }

    [StringLength(100, ErrorMessage = "La {0} no puede exceder {1} caracteres")]
    public string? referencia_pago { get; set; }
}

/// <summary>
/// DTO para actualización de pagos de factura
/// </summary>
public class PagoFacturaUpdateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int pago_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int factura_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int id_metodo_pago { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El {0} debe ser mayor a 0")]
    public decimal monto { get; set; }

    [StringLength(100, ErrorMessage = "La {0} no puede exceder {1} caracteres")]
    public string? referencia_pago { get; set; }
}
