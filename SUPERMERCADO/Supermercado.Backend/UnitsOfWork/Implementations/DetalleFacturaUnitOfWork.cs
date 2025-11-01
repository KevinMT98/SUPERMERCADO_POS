using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unit of Work para manejar la lógica de negocio de Detalle de Factura
/// </summary>
public class DetalleFacturaUnitOfWork : GenericUnitOfWork<Detalle_Factura>, IDetalleFacturaUnitOfWork
{
    private readonly IDetalleFacturaRepository _detalleFacturaRepository;
    private readonly IFacturaRepository _facturaRepository;
    private readonly IProductoRepository _productoRepository;

    public DetalleFacturaUnitOfWork(
        IDetalleFacturaRepository detalleFacturaRepository,
        IFacturaRepository facturaRepository,
        IProductoRepository productoRepository) : base(detalleFacturaRepository)
    {
        _detalleFacturaRepository = detalleFacturaRepository;
        _facturaRepository = facturaRepository;
        _productoRepository = productoRepository;
    }

    /// <summary>
    /// Validaciones de negocio antes de agregar un detalle de factura
    /// </summary>
    public override async Task<ActionResponse<Detalle_Factura>> AddAsync(Detalle_Factura entity)
    {
        // Validar que la factura exista
        var facturaResult = await _facturaRepository.GetAsync(entity.FK_factura_id);
        if (!facturaResult.WasSuccess)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "La factura especificada no existe."
            };
        }

        // Validar que el producto exista
        var productoResult = await _productoRepository.GetAsync(entity.FK_producto_id);
        if (!productoResult.WasSuccess)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "El producto especificado no existe."
            };
        }

        var producto = productoResult.Result!;

        // Validar que el producto esté activo
        if (!producto.activo)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = $"El producto '{producto.nombre}' no está activo."
            };
        }

        // Validar que haya stock suficiente
        if (producto.stock_actual < entity.cantidad)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = $"Stock insuficiente para el producto '{producto.nombre}'. Stock disponible: {producto.stock_actual}"
            };
        }

        // Validar que la cantidad sea positiva
        if (entity.cantidad <= 0)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "La cantidad debe ser mayor a 0."
            };
        }

        // Validar que el precio unitario sea positivo
        if (entity.precio_unitario < 0)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "El precio unitario no puede ser negativo."
            };
        }

        // Validar que el porcentaje de descuento esté en el rango correcto
        if (entity.descuento_porcentaje < 0 || entity.descuento_porcentaje > 100)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "El porcentaje de descuento debe estar entre 0 y 100."
            };
        }

        // Validar el cálculo del subtotal
        var subtotalCalculado = (entity.cantidad * entity.precio_unitario) - entity.descuento_valor;
        if (Math.Abs(entity.subtotal - subtotalCalculado) > 0.01m) // Tolerancia de 1 centavo
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = $"El subtotal ({entity.subtotal}) no coincide con el cálculo (cantidad * precio - descuento = {subtotalCalculado})."
            };
        }

        // Validar que el descuento no sea mayor que el total de la línea
        var totalLinea = entity.cantidad * entity.precio_unitario;
        if (entity.descuento_valor > totalLinea)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "El descuento no puede ser mayor que el total de la línea."
            };
        }

        return await _detalleFacturaRepository.AddAsync(entity);
    }

    /// <summary>
    /// Validaciones de negocio antes de actualizar un detalle de factura
    /// </summary>
    public override async Task<ActionResponse<Detalle_Factura>> UpdateAsync(Detalle_Factura entity)
    {
        // Validar que el detalle exista
        var existingDetalle = await _detalleFacturaRepository.GetAsync(entity.detalle_id);
        if (!existingDetalle.WasSuccess)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "El detalle de factura especificado no existe."
            };
        }

        // Aplicar las mismas validaciones que en AddAsync
        var facturaResult = await _facturaRepository.GetAsync(entity.FK_factura_id);
        if (!facturaResult.WasSuccess)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "La factura especificada no existe."
            };
        }

        var productoResult = await _productoRepository.GetAsync(entity.FK_producto_id);
        if (!productoResult.WasSuccess)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "El producto especificado no existe."
            };
        }

        var producto = productoResult.Result!;

        if (!producto.activo)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = $"El producto '{producto.nombre}' no está activo."
            };
        }

        if (entity.cantidad <= 0)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "La cantidad debe ser mayor a 0."
            };
        }

        if (entity.precio_unitario < 0)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "El precio unitario no puede ser negativo."
            };
        }

        if (entity.descuento_porcentaje < 0 || entity.descuento_porcentaje > 100)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "El porcentaje de descuento debe estar entre 0 y 100."
            };
        }

        var subtotalCalculado = (entity.cantidad * entity.precio_unitario) - entity.descuento_valor;
        if (Math.Abs(entity.subtotal - subtotalCalculado) > 0.01m)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = $"El subtotal ({entity.subtotal}) no coincide con el cálculo (cantidad * precio - descuento = {subtotalCalculado})."
            };
        }

        var totalLinea = entity.cantidad * entity.precio_unitario;
        if (entity.descuento_valor > totalLinea)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "El descuento no puede ser mayor que el total de la línea."
            };
        }

        return await _detalleFacturaRepository.UpdateAsync(entity);
    }
}
