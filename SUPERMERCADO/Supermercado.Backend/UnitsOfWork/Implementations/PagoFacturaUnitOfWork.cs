using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unit of Work para manejar la lógica de negocio de Pago de Factura
/// </summary>
public class PagoFacturaUnitOfWork : GenericUnitOfWork<Pago_Factura>, IPagoFacturaUnitOfWork
{
    private readonly IPagoFacturaRepository _pagoFacturaRepository;
    private readonly IFacturaRepository _facturaRepository;
    private readonly IMetodosPagoRepository _metodosPagoRepository;

    public PagoFacturaUnitOfWork(
        IPagoFacturaRepository pagoFacturaRepository,
        IFacturaRepository facturaRepository,
        IMetodosPagoRepository metodosPagoRepository) : base(pagoFacturaRepository)
    {
        _pagoFacturaRepository = pagoFacturaRepository;
        _facturaRepository = facturaRepository;
        _metodosPagoRepository = metodosPagoRepository;
    }

    /// <summary>
    /// Validaciones de negocio antes de agregar un pago de factura
    /// </summary>
    public override async Task<ActionResponse<Pago_Factura>> AddAsync(Pago_Factura entity)
    {
        // Validar que la factura exista
        var facturaResult = await _facturaRepository.GetAsync(entity.FK_factura_id);
        if (!facturaResult.WasSuccess)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = "La factura especificada no existe."
            };
        }

        // Validar que el método de pago exista
        var metodoPagoResult = await _metodosPagoRepository.GetAsync(entity.FK_id_metodo_pago);
        if (!metodoPagoResult.WasSuccess)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = "El método de pago especificado no existe."
            };
        }

        var metodoPago = metodoPagoResult.Result!;

        // Validar que el método de pago esté activo
        if (!metodoPago.activo)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = $"El método de pago '{metodoPago.metodo_pago}' no está activo."
            };
        }

        // Validar que el monto sea positivo
        if (entity.monto <= 0)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = "El monto del pago debe ser mayor a 0."
            };
        }

        // Aquí se podría agregar validación adicional para verificar que la suma de pagos no exceda el total de la factura
        // Por ejemplo:
        // var totalPagado = await CalcularTotalPagadoFacturaAsync(entity.FK_factura_id);
        // if (totalPagado + entity.monto > facturaResult.Result.total_neto)
        // {
        //     return new ActionResponse<Pago_Factura>
        //     {
        //         WasSuccess = false,
        //         Message = "El monto total de los pagos excede el total de la factura."
        //     };
        // }

        return await _pagoFacturaRepository.AddAsync(entity);
    }

    /// <summary>
    /// Validaciones de negocio antes de actualizar un pago de factura
    /// </summary>
    public override async Task<ActionResponse<Pago_Factura>> UpdateAsync(Pago_Factura entity)
    {
        // Validar que el pago exista
        var existingPago = await _pagoFacturaRepository.GetAsync(entity.pago_id);
        if (!existingPago.WasSuccess)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = "El pago de factura especificado no existe."
            };
        }

        // Aplicar las mismas validaciones que en AddAsync
        var facturaResult = await _facturaRepository.GetAsync(entity.FK_factura_id);
        if (!facturaResult.WasSuccess)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = "La factura especificada no existe."
            };
        }

        var metodoPagoResult = await _metodosPagoRepository.GetAsync(entity.FK_id_metodo_pago);
        if (!metodoPagoResult.WasSuccess)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = "El método de pago especificado no existe."
            };
        }

        var metodoPago = metodoPagoResult.Result!;

        if (!metodoPago.activo)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = $"El método de pago '{metodoPago.metodo_pago}' no está activo."
            };
        }

        if (entity.monto <= 0)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = "El monto del pago debe ser mayor a 0."
            };
        }

        return await _pagoFacturaRepository.UpdateAsync(entity);
    }
}
