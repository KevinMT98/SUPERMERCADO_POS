using Supermercado.Shared.DTOs;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;

/// <summary>
/// Unit of Work para operaciones de facturación
/// </summary>
public interface IFacturacionUnitOfWork
{
    /// <summary>
    /// Crea una factura completa con todas las validaciones de negocio
    /// </summary>
    Task<ActionResponse<FacturaCompletaDTO>> CrearFacturaCompletaAsync(FacturaCompletaCreateDTO facturaDto);

    /// <summary>
    /// Obtiene una factura completa con todos sus detalles
    /// </summary>
    Task<ActionResponse<FacturaCompletaDTO>> ObtenerFacturaCompletaAsync(int facturaId);

    /// <summary>
    /// Obtiene facturas con filtros aplicados
    /// </summary>
    Task<ActionResponse<IEnumerable<FacturaCompletaDTO>>> ObtenerFacturasConFiltrosAsync(FacturaFiltroDTO filtros);

    /// <summary>
    /// Anula una factura con validaciones de negocio
    /// </summary>
    Task<ActionResponse<bool>> AnularFacturaAsync(int facturaId, int usuarioId, string motivo);

    /// <summary>
    /// Obtiene el resumen de ventas por fecha
    /// </summary>
    Task<ActionResponse<ResumenVentasDTO>> ObtenerResumenVentasAsync(DateTime fecha);

    /// <summary>
    /// Obtiene facturas pendientes de pago
    /// </summary>
    Task<ActionResponse<IEnumerable<FacturaCompletaDTO>>> ObtenerFacturasPendientesPagoAsync();

    /// <summary>
    /// Valida los datos de una factura antes de crearla
    /// </summary>
    Task<ActionResponse<bool>> ValidarDatosFacturaAsync(FacturaCompletaCreateDTO facturaDto);

    /// <summary>
    /// Obtiene productos disponibles para facturación
    /// </summary>
    Task<ActionResponse<IEnumerable<ProductoDto>>> ObtenerProductosDisponiblesAsync();

    /// <summary>
    /// Obtiene métodos de pago activos
    /// </summary>
    Task<ActionResponse<IEnumerable<MetodosPagoDTO>>> ObtenerMetodosPagoActivosAsync();

    /// <summary>
    /// Obtiene terceros activos (clientes)
    /// </summary>
    Task<ActionResponse<IEnumerable<TerceroDTO>>> ObtenerClientesActivosAsync();
}
