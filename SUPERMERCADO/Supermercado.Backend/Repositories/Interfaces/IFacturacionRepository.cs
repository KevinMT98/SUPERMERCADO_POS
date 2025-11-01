using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Interfaces;

/// <summary>
/// Interfaz para el repositorio de facturación con operaciones complejas
/// </summary>
public interface IFacturacionRepository
{
    /// <summary>
    /// Crea una factura completa con movimiento, detalles y pagos
    /// </summary>
    Task<ActionResponse<FacturaCompletaDTO>> CrearFacturaCompletaAsync(FacturaCompletaCreateDTO facturaDto);

    /// <summary>
    /// Obtiene una factura completa con todos sus detalles
    /// </summary>
    Task<ActionResponse<FacturaCompletaDTO>> ObtenerFacturaCompletaAsync(int facturaId);

    /// <summary>
    /// Obtiene facturas con filtros
    /// </summary>
    Task<ActionResponse<IEnumerable<FacturaCompletaDTO>>> ObtenerFacturasConFiltrosAsync(FacturaFiltroDTO filtros);

    /// <summary>
    /// Anula una factura (marca como anulada)
    /// </summary>
    Task<ActionResponse<bool>> AnularFacturaAsync(int facturaId, int usuarioId, string motivo);

    /// <summary>
    /// Obtiene el resumen de ventas por fecha
    /// </summary>
    Task<ActionResponse<ResumenVentasDTO>> ObtenerResumenVentasAsync(DateTime fecha);

    /// <summary>
    /// Valida que haya suficiente stock para los productos
    /// </summary>
    Task<ActionResponse<bool>> ValidarStockProductosAsync(List<DetalleFacturaItemDTO> detalles);

    /// <summary>
    /// Actualiza el stock de los productos después de la venta
    /// </summary>
    Task<ActionResponse<bool>> ActualizarStockProductosAsync(List<DetalleFacturaItemDTO> detalles);

    /// <summary>
    /// Obtiene las facturas pendientes de pago
    /// </summary>
    Task<ActionResponse<IEnumerable<FacturaCompletaDTO>>> ObtenerFacturasPendientesPagoAsync();

    /// <summary>
    /// Calcula los totales de una factura incluyendo descuentos e impuestos
    /// </summary>
    Task<ActionResponse<(decimal totalBruto, decimal totalDescuentos, decimal totalImpuestos, decimal totalNeto)>> CalcularTotalesFacturaAsync(List<DetalleFacturaItemDTO> detalles);
}
