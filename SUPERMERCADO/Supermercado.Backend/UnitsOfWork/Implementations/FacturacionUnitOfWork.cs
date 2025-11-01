using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Implementación del Unit of Work para facturación
/// </summary>
public class FacturacionUnitOfWork : IFacturacionUnitOfWork
{
    private readonly IFacturacionRepository _facturacionRepository;
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public FacturacionUnitOfWork(
        IFacturacionRepository facturacionRepository,
        DataContext context,
        IMapper mapper)
    {
        _facturacionRepository = facturacionRepository;
        _context = context;
        _mapper = mapper;
    }

    public async Task<ActionResponse<FacturaCompletaDTO>> CrearFacturaCompletaAsync(FacturaCompletaCreateDTO facturaDto)
    {
        // 1. Validar datos de entrada
        var validacion = await ValidarDatosFacturaAsync(facturaDto);
        if (!validacion.WasSuccess)
        {
            return new ActionResponse<FacturaCompletaDTO>
            {
                WasSuccess = false,
                Message = validacion.Message
            };
        }

        // 2. Validar reglas de negocio específicas
        var validacionNegocio = await ValidarReglasNegocioAsync(facturaDto);
        if (!validacionNegocio.WasSuccess)
        {
            return new ActionResponse<FacturaCompletaDTO>
            {
                WasSuccess = false,
                Message = validacionNegocio.Message
            };
        }

        // 3. Crear la factura
        return await _facturacionRepository.CrearFacturaCompletaAsync(facturaDto);
    }

    public async Task<ActionResponse<FacturaCompletaDTO>> ObtenerFacturaCompletaAsync(int facturaId)
    {
        if (facturaId <= 0)
        {
            return new ActionResponse<FacturaCompletaDTO>
            {
                WasSuccess = false,
                Message = "El ID de la factura debe ser mayor a 0"
            };
        }

        return await _facturacionRepository.ObtenerFacturaCompletaAsync(facturaId);
    }

    public async Task<ActionResponse<IEnumerable<FacturaCompletaDTO>>> ObtenerFacturasConFiltrosAsync(FacturaFiltroDTO filtros)
    {
        // Validar rangos de fechas
        if (filtros.FechaInicio.HasValue && filtros.FechaFin.HasValue)
        {
            if (filtros.FechaInicio > filtros.FechaFin)
            {
                return new ActionResponse<IEnumerable<FacturaCompletaDTO>>
                {
                    WasSuccess = false,
                    Message = "La fecha de inicio no puede ser mayor a la fecha fin"
                };
            }
        }

        // Validar rangos de montos
        if (filtros.MontoMinimo.HasValue && filtros.MontoMaximo.HasValue)
        {
            if (filtros.MontoMinimo > filtros.MontoMaximo)
            {
                return new ActionResponse<IEnumerable<FacturaCompletaDTO>>
                {
                    WasSuccess = false,
                    Message = "El monto mínimo no puede ser mayor al monto máximo"
                };
            }
        }

        return await _facturacionRepository.ObtenerFacturasConFiltrosAsync(filtros);
    }

    public async Task<ActionResponse<bool>> AnularFacturaAsync(int facturaId, int usuarioId, string motivo)
    {
        if (facturaId <= 0)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = "El ID de la factura debe ser mayor a 0"
            };
        }

        if (usuarioId <= 0)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = "El ID del usuario debe ser mayor a 0"
            };
        }

        if (string.IsNullOrWhiteSpace(motivo))
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = "El motivo de anulación es obligatorio"
            };
        }

        // Validar que la factura se pueda anular (ej: no más de X días)
        var factura = await _context.Facturas
            .Include(f => f.Movimiento)
            .FirstOrDefaultAsync(f => f.factura_id == facturaId);

        if (factura == null)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = "Factura no encontrada"
            };
        }

        // Regla de negocio: No se pueden anular facturas de más de 30 días
        var diasTranscurridos = (DateTime.Now - factura.Movimiento!.fecha).Days;
        if (diasTranscurridos > 30)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = "No se pueden anular facturas de más de 30 días"
            };
        }

        return await _facturacionRepository.AnularFacturaAsync(facturaId, usuarioId, motivo);
    }

    public async Task<ActionResponse<ResumenVentasDTO>> ObtenerResumenVentasAsync(DateTime fecha)
    {
        return await _facturacionRepository.ObtenerResumenVentasAsync(fecha);
    }

    public async Task<ActionResponse<IEnumerable<FacturaCompletaDTO>>> ObtenerFacturasPendientesPagoAsync()
    {
        return await _facturacionRepository.ObtenerFacturasPendientesPagoAsync();
    }

    public async Task<ActionResponse<bool>> ValidarDatosFacturaAsync(FacturaCompletaCreateDTO facturaDto)
    {
        try
        {
            // Validar que exista el tercero
            var tercero = await _context.Terceros
                .FirstOrDefaultAsync(t => t.tercero_id == facturaDto.TerceroId && t.activo);

            if (tercero == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "El tercero especificado no existe o está inactivo"
                };
            }

            // Validar que el tercero sea cliente
            if (!tercero.es_cliente)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "El tercero especificado no está marcado como cliente"
                };
            }

            // Validar que exista el usuario
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.usuario_id == facturaDto.UsuarioId && u.activo);

            if (usuario == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "El usuario especificado no existe o está inactivo"
                };
            }

            // Validar productos en los detalles
            foreach (var detalle in facturaDto.Detalles)
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.producto_id == detalle.ProductoId && p.activo);

                if (producto == null)
                {
                    return new ActionResponse<bool>
                    {
                        WasSuccess = false,
                        Message = $"El producto con ID {detalle.ProductoId} no existe o está inactivo"
                    };
                }
            }

            // Validar métodos de pago
            foreach (var pago in facturaDto.Pagos)
            {
                var metodoPago = await _context.MetodosPago
                    .FirstOrDefaultAsync(m => m.id_metodo_pago == pago.MetodoPagoId && m.activo);

                if (metodoPago == null)
                {
                    return new ActionResponse<bool>
                    {
                        WasSuccess = false,
                        Message = $"El método de pago con ID {pago.MetodoPagoId} no existe o está inactivo"
                    };
                }
            }

            return new ActionResponse<bool>
            {
                WasSuccess = true,
                Result = true
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = $"Error al validar datos: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<ProductoDto>>> ObtenerProductosDisponiblesAsync()
    {
        try
        {
            var productos = await _context.Productos
                .Where(p => p.activo && p.stock_actual > 0)
                .ToListAsync();

            var productosDto = _mapper.Map<IEnumerable<ProductoDto>>(productos);

            return new ActionResponse<IEnumerable<ProductoDto>>
            {
                WasSuccess = true,
                Result = productosDto
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<IEnumerable<ProductoDto>>
            {
                WasSuccess = false,
                Message = $"Error al obtener productos: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<MetodosPagoDTO>>> ObtenerMetodosPagoActivosAsync()
    {
        try
        {
            var metodosPago = await _context.MetodosPago
                .Where(m => m.activo)
                .ToListAsync();

            var metodosPagoDto = _mapper.Map<IEnumerable<MetodosPagoDTO>>(metodosPago);

            return new ActionResponse<IEnumerable<MetodosPagoDTO>>
            {
                WasSuccess = true,
                Result = metodosPagoDto
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<IEnumerable<MetodosPagoDTO>>
            {
                WasSuccess = false,
                Message = $"Error al obtener métodos de pago: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<TerceroDTO>>> ObtenerClientesActivosAsync()
    {
        try
        {
            var clientes = await _context.Terceros
                .Where(t => t.activo && t.es_cliente)
                .ToListAsync();

            var clientesDto = _mapper.Map<IEnumerable<TerceroDTO>>(clientes);

            return new ActionResponse<IEnumerable<TerceroDTO>>
            {
                WasSuccess = true,
                Result = clientesDto
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<IEnumerable<TerceroDTO>>
            {
                WasSuccess = false,
                Message = $"Error al obtener clientes: {ex.Message}"
            };
        }
    }

    private async Task<ActionResponse<bool>> ValidarReglasNegocioAsync(FacturaCompletaCreateDTO facturaDto)
    {
        try
        {
            // Regla 1: Validar que los descuentos no excedan el subtotal
            foreach (var detalle in facturaDto.Detalles)
            {
                var subtotalBruto = detalle.Cantidad * detalle.PrecioUnitario;
                if (detalle.DescuentoValor > subtotalBruto)
                {
                    return new ActionResponse<bool>
                    {
                        WasSuccess = false,
                        Message = $"El descuento no puede ser mayor al subtotal del producto ID {detalle.ProductoId}"
                    };
                }
            }

            // Regla 2: Validar que no haya productos duplicados
            var productosIds = facturaDto.Detalles.Select(d => d.ProductoId).ToList();
            if (productosIds.Count != productosIds.Distinct().Count())
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "No se pueden incluir productos duplicados en la misma factura"
                };
            }

            // Regla 3: Validar monto mínimo de factura (ejemplo: $1000)
            var totalesResponse = await _facturacionRepository.CalcularTotalesFacturaAsync(facturaDto.Detalles);
            if (totalesResponse.WasSuccess)
            {
                var (_, _, _, totalNeto) = totalesResponse.Result;
                if (totalNeto < 1000)
                {
                    return new ActionResponse<bool>
                    {
                        WasSuccess = false,
                        Message = "El monto mínimo de facturación es $1,000"
                    };
                }
            }

            // Regla 4: Validar horario de facturación (ejemplo: 6 AM a 10 PM)
            //var horaActual = DateTime.Now.Hour;
            //if (horaActual < 6 || horaActual > 22)
            //{
            //    return new ActionResponse<bool>
            //    {
            //        WasSuccess = false,
            //        Message = "La facturación solo está permitida entre las 6:00 AM y las 10:00 PM"
            //    };
            //}

            return new ActionResponse<bool>
            {
                WasSuccess = true,
                Result = true
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = $"Error al validar reglas de negocio: {ex.Message}"
            };
        }
    }
}
