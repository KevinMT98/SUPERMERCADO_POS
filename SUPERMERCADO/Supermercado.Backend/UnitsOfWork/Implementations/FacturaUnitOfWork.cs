using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unit of Work para manejar la lógica de negocio de Facturas
/// </summary>
public class FacturaUnitOfWork : GenericUnitOfWork<Factura>, IFacturaUnitOfWork
{
    private readonly IFacturaRepository _facturaRepository;
    private readonly IMovimientoRepository _movimientoRepository;

    public FacturaUnitOfWork(
        IFacturaRepository facturaRepository,
        IMovimientoRepository movimientoRepository) : base(facturaRepository)
    {
        _facturaRepository = facturaRepository;
        _movimientoRepository = movimientoRepository;
    }

    /// <summary>
    /// Validaciones de negocio antes de agregar una factura
    /// </summary>
    public override async Task<ActionResponse<Factura>> AddAsync(Factura entity)
    {
        // Validar que el movimiento exista
        var movimientoResult = await _movimientoRepository.GetAsync(entity.FK_movimiento_id);
        if (!movimientoResult.WasSuccess)
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = "El movimiento especificado no existe."
            };
        }

        // Validar que el total neto sea correcto (total_bruto - total_descuentos)
        var totalCalculado = entity.total_bruto - entity.total_descuentos;
        if (Math.Abs(entity.total_neto - totalCalculado) > 0.01m) // Tolerancia de 1 centavo
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = $"El total neto ({entity.total_neto}) no coincide con el cálculo (total bruto - descuentos = {totalCalculado})."
            };
        }

        // Validar que los totales sean positivos o cero
        if (entity.total_bruto < 0 || entity.total_descuentos < 0 || entity.total_neto < 0)
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = "Los totales no pueden ser negativos."
            };
        }

        // Validar que el descuento no sea mayor que el total bruto
        if (entity.total_descuentos > entity.total_bruto)
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = "El total de descuentos no puede ser mayor que el total bruto."
            };
        }

        return await _facturaRepository.AddAsync(entity);
    }

    /// <summary>
    /// Validaciones de negocio antes de actualizar una factura
    /// </summary>
    public override async Task<ActionResponse<Factura>> UpdateAsync(Factura entity)
    {
        // Validar que la factura exista
        var existingFactura = await _facturaRepository.GetAsync(entity.factura_id);
        if (!existingFactura.WasSuccess)
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = "La factura especificada no existe."
            };
        }

        // Aplicar las mismas validaciones que en AddAsync
        var movimientoResult = await _movimientoRepository.GetAsync(entity.FK_movimiento_id);
        if (!movimientoResult.WasSuccess)
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = "El movimiento especificado no existe."
            };
        }

        var totalCalculado = entity.total_bruto - entity.total_descuentos;
        if (Math.Abs(entity.total_neto - totalCalculado) > 0.01m)
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = $"El total neto ({entity.total_neto}) no coincide con el cálculo (total bruto - descuentos = {totalCalculado})."
            };
        }

        if (entity.total_bruto < 0 || entity.total_descuentos < 0 || entity.total_neto < 0)
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = "Los totales no pueden ser negativos."
            };
        }

        if (entity.total_descuentos > entity.total_bruto)
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = "El total de descuentos no puede ser mayor que el total bruto."
            };
        }

        return await _facturaRepository.UpdateAsync(entity);
    }
}
