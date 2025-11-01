using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.DTOs;

/// <summary>
/// DTO para lectura de facturas
/// </summary>
public class FacturaDTO
{
    public int factura_id { get; set; }
    public int movimiento_id { get; set; }
    public decimal total_bruto { get; set; }
    public decimal total_descuentos { get; set; }
    public decimal total_neto { get; set; }
}

/// <summary>
/// DTO para creación de facturas
/// </summary>
public class FacturaCreateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int movimiento_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal total_bruto { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal total_descuentos { get; set; } = 0;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal total_neto { get; set; }
}

/// <summary>
/// DTO para actualización de facturas
/// </summary>
public class FacturaUpdateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int factura_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int movimiento_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal total_bruto { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal total_descuentos { get; set; } = 0;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal total_neto { get; set; }
}
