using System.ComponentModel.DataAnnotations;

namespace Supermercado.Shared.DTOs;

/// <summary>
/// DTO para un item de detalle de factura
/// </summary>
public class DetalleFacturaItemDTO
{
    [Required(ErrorMessage = "El producto es obligatorio")]
    public int ProductoId { get; set; }

    [Required(ErrorMessage = "La cantidad es obligatoria")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
    public int Cantidad { get; set; }

    [Required(ErrorMessage = "El precio unitario es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0")]
    public decimal PrecioUnitario { get; set; }

    [Range(0, 100, ErrorMessage = "El descuento porcentaje debe estar entre 0 y 100")]
    public decimal DescuentoPorcentaje { get; set; } = 0;

    [Range(0, double.MaxValue, ErrorMessage = "El descuento valor debe ser mayor o igual a 0")]
    public decimal DescuentoValor { get; set; } = 0;
}

/// <summary>
/// DTO para un item de pago de factura
/// </summary>
public class PagoFacturaItemDTO
{
    [Required(ErrorMessage = "El método de pago es obligatorio")]
    public int MetodoPagoId { get; set; }

    [Required(ErrorMessage = "El monto es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a 0")]
    public decimal Monto { get; set; }

    [MaxLength(100, ErrorMessage = "La referencia no puede exceder {1} caracteres")]
    public string? ReferenciaPago { get; set; }
}

/// <summary>
/// DTO para crear una factura completa con todos sus detalles y pagos
/// </summary>
public class FacturaCompletaCreateDTO
{
    [Required(ErrorMessage = "El tercero es obligatorio")]
    public int TerceroId { get; set; }

    [Required(ErrorMessage = "El usuario es obligatorio")]
    public int UsuarioId { get; set; }

    [MaxLength(500, ErrorMessage = "Las observaciones no pueden exceder {1} caracteres")]
    public string? Observaciones { get; set; }

    [Required(ErrorMessage = "Los detalles de la factura son obligatorios")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un producto")]
    public List<DetalleFacturaItemDTO> Detalles { get; set; } = new();

    [Required(ErrorMessage = "Los pagos son obligatorios")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un método de pago")]
    public List<PagoFacturaItemDTO> Pagos { get; set; } = new();
}

/// <summary>
/// DTO de respuesta para factura completa
/// </summary>
public class FacturaCompletaDTO
{
    public int FacturaId { get; set; }
    //public int MovimientoId { get; set; }
    public string NumeroDocumento { get; set; } = null!;
    public DateTime Fecha { get; set; }
   // public int TerceroId { get; set; }
    public string NombreTercero { get; set; } = null!;
    //public int UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = null!;
    public string? Observaciones { get; set; }
    public decimal TotalBruto { get; set; }
    public decimal TotalImpu { get; set; }
    public decimal TotalDescuentos { get; set; }
    public decimal TotalNeto { get; set; }
    public List<DetalleFacturaCompletaDTO> Detalles { get; set; } = new();
    public List<PagoFacturaCompletaDTO> Pagos { get; set; } = new();
}

/// <summary>
/// DTO de detalle de factura con información del producto
/// </summary>
public class DetalleFacturaCompletaDTO
{
    public int DetalleId { get; set; }
    public int ProductoId { get; set; }
    public string CodigoProducto { get; set; } = null!;
    public string NombreProducto { get; set; } = null!;
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PorcentajeIva { get; set; }
    public decimal DescuentoPorcentaje { get; set; }
    public decimal DescuentoValor { get; set; }
    public decimal Subtotal { get; set; }
}

/// <summary>
/// DTO de pago de factura con información del método de pago
/// </summary>
public class PagoFacturaCompletaDTO
{
    public int PagoId { get; set; }
    public int MetodoPagoId { get; set; }
    public string NombreMetodoPago { get; set; } = null!;
    public decimal Monto { get; set; }
    public string? ReferenciaPago { get; set; }
}

/// <summary>
/// DTO para consultar facturas con filtros
/// </summary>
public class FacturaFiltroDTO
{
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int? TerceroId { get; set; }
    public int? UsuarioId { get; set; }
    public decimal? MontoMinimo { get; set; }
    public decimal? MontoMaximo { get; set; }
    public string? NumeroDocumento { get; set; }
}

/// <summary>
/// DTO para resumen de ventas
/// </summary>
public class ResumenVentasDTO
{
    public DateTime Fecha { get; set; }
    public int TotalFacturas { get; set; }
    public decimal TotalVentas { get; set; }
    public decimal TotalDescuentos { get; set; }
    public decimal VentaNeta { get; set; }
    public List<VentaPorMetodoPagoDTO> VentasPorMetodoPago { get; set; } = new();
}

/// <summary>
/// DTO para ventas por método de pago
/// </summary>
public class VentaPorMetodoPagoDTO
{
    public int MetodoPagoId { get; set; }
    public string NombreMetodoPago { get; set; } = null!;
    public decimal TotalVentas { get; set; }
    public int CantidadTransacciones { get; set; }
}

/// <summary>
/// DTO para anular una factura
/// </summary>
public class AnularFacturaDTO
{
    [Required(ErrorMessage = "El usuario que anula es obligatorio")]
    public int UsuarioId { get; set; }

    [Required(ErrorMessage = "El motivo de anulación es obligatorio")]
    [MaxLength(500, ErrorMessage = "El motivo no puede exceder {1} caracteres")]
    public string Motivo { get; set; } = null!;
}
