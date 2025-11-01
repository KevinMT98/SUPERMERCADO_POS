using Supermercado.Shared.Entities;
using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.DTOs;


/// <summary>
/// DTO para lectura de detalles de factura
/// </summary>
public class DetalleFacturaDTO
{
    public int detalle_id { get; set; }
    public int factura_id { get; set; }
    public int producto_id { get; set; }
    public int cantidad { get; set; }
    public decimal precio_unitario { get; set; }
    public decimal porcentaje_iva { get; set; }
    public decimal descuento_porcentaje { get; set; }
    public decimal descuento_valor { get; set; }
    public decimal subtotal { get; set; }
}

/// <summary>
/// DTO para creación de detalles de factura
/// </summary>
public class DetalleFacturaCreateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int factura_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int producto_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "La {0} debe ser mayor a 0")]
    public int cantidad { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal precio_unitario { get; set; }

    [Range(0, 100, ErrorMessage = "El {0} debe estar entre 0 y 100")]
    public decimal descuento_porcentaje { get; set; } = 0;

    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal descuento_valor { get; set; } = 0;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal subtotal { get; set; }
}

/// <summary>
/// DTO para actualización de detalles de factura
/// </summary>
public class DetalleFacturaUpdateDTO
{
    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int detalle_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int factura_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    public int producto_id { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "La {0} debe ser mayor a 0")]
    public int cantidad { get; set; }

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal precio_unitario { get; set; }

    [Range(0, 100, ErrorMessage = "El {0} debe estar entre 0 y 100")]
    public decimal descuento_porcentaje { get; set; } = 0;

    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal descuento_valor { get; set; } = 0;

    [Required(ErrorMessage = "El campo {0} es obligatorio")]
    [Range(0, double.MaxValue, ErrorMessage = "El {0} debe ser mayor o igual a 0")]
    public decimal subtotal { get; set; }
}
