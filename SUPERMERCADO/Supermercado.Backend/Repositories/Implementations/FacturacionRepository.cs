using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Implementación del repositorio de facturación
/// </summary>
public class FacturacionRepository : IFacturacionRepository
{
    private readonly DataContext _context;

    public FacturacionRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ActionResponse<FacturaCompletaDTO>> CrearFacturaCompletaAsync(FacturaCompletaCreateDTO facturaDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. Validar stock de productos
            var stockValidation = await ValidarStockProductosAsync(facturaDto.Detalles);
            if (!stockValidation.WasSuccess)
            {
                return new ActionResponse<FacturaCompletaDTO>
                {
                    WasSuccess = false,
                    Message = stockValidation.Message
                };
            }

            // 2. Obtener el tipo de documento para facturas de venta (código "FA")
            var tipoDocumento = await _context.tipoDctos.FirstOrDefaultAsync(t => t.Codigo == "FA");
            if (tipoDocumento == null)
            {
                return new ActionResponse<FacturaCompletaDTO>
                {
                    WasSuccess = false,
                    Message = "No se encontró el tipo de documento para facturas de venta"
                };
            }

            // 3. Obtener el consecutivo para facturas de venta
            var consecutivo = await _context.consecutivos
                .FirstOrDefaultAsync(c => c.cod_consecut == "FV");

            if (consecutivo == null)
            {
                return new ActionResponse<FacturaCompletaDTO>
                {
                    WasSuccess = false,
                    Message = "No se encontró configuración de consecutivo para facturas"
                };
            }

            // 4. Validar que el consecutivo no haya excedido el límite
            if (consecutivo.consecutivo_actual >= consecutivo.consecutivo_fin)
            {
                return new ActionResponse<FacturaCompletaDTO>
                {
                    WasSuccess = false,
                    Message = $"Se ha excedido el límite del consecutivo (máximo: {consecutivo.consecutivo_fin})"
                };
            }

            // 5. Obtener siguiente número de documento
            var siguienteNumero = consecutivo.consecutivo_actual + 1;
            var numeroDocumento = $"FV{siguienteNumero:D6}"; // Formato: FV000001

            // 6. Calcular totales
            var totalesResponse = await CalcularTotalesFacturaAsync(facturaDto.Detalles);
            if (!totalesResponse.WasSuccess)
            {
                return new ActionResponse<FacturaCompletaDTO>
                {
                    WasSuccess = false,
                    Message = "Error al calcular los totales"
                };
            }

            var (totalBruto, totalDescuentos, totalImpuestos, totalNeto) = totalesResponse.Result;

            // 7. Validar que los pagos cubran el total
            var totalPagos = facturaDto.Pagos.Sum(p => p.Monto);
            if (Math.Abs(totalPagos - totalNeto) > 0.01m)
            {
                return new ActionResponse<FacturaCompletaDTO>
                {
                    WasSuccess = false,
                    Message = $"El total de pagos ({totalPagos:C}) no coincide con el total de la factura ({totalNeto:C})"
                };
            }

            // 8. Crear movimiento
            var movimiento = new Movimiento
            {
                FK_codigo_tipodoc = tipoDocumento.ID,
                FK_consecutivo_id = consecutivo.consecutivo_Id,
                numero_documento = numeroDocumento,
                fecha = DateTime.Now,
                FK_usuario_id = facturaDto.UsuarioId,
                FK_tercero_id = facturaDto.TerceroId,
                observaciones = facturaDto.Observaciones
            };

            _context.Movimientos.Add(movimiento);
            await _context.SaveChangesAsync();

            // 9. Crear factura
            var factura = new Factura
            {
                FK_movimiento_id = movimiento.movimiento_id,
                total_bruto = totalBruto,
                total_descuentos = totalDescuentos,
                total_impu = totalImpuestos,
                total_neto = totalNeto
            };

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            // 10. Crear detalles de factura
            var detalles = new List<Detalle_Factura>();
            foreach (var detalle in facturaDto.Detalles)
            {
                // Obtener el producto con su tarifa de IVA
                var producto = await _context.Productos
                    .Include(p => p.TarifaIVA)
                    .FirstOrDefaultAsync(p => p.producto_id == detalle.ProductoId);

                if (producto == null)
                {
                    await transaction.RollbackAsync();
                    return new ActionResponse<FacturaCompletaDTO>
                    {
                        WasSuccess = false,
                        Message = $"Producto con ID {detalle.ProductoId} no encontrado"
                    };
                }

                // Calcular valores del detalle
                var calculos = CalcularValoresDetalle(
                    detalle.Cantidad,
                    detalle.PrecioUnitario,
                    detalle.DescuentoPorcentaje,
                    detalle.DescuentoValor,
                    producto.TarifaIVA?.porcentaje ?? 0
                );

                var detalleFactura = new Detalle_Factura
                {
                    FK_factura_id = factura.factura_id,
                    FK_producto_id = detalle.ProductoId,
                    cantidad = detalle.Cantidad,
                    precio_unitario = detalle.PrecioUnitario,
                    descuento_porcentaje = detalle.DescuentoPorcentaje,
                    descuento_valor = calculos.DescuentoValor,
                    subtotal = calculos.Subtotal
                };

                detalles.Add(detalleFactura);
                _context.DetallesFactura.Add(detalleFactura);
            }

            await _context.SaveChangesAsync();

            // 11. Crear pagos de factura
            foreach (var pago in facturaDto.Pagos)
            {
                var pagoFactura = new Pago_Factura
                {
                    FK_factura_id = factura.factura_id,
                    FK_id_metodo_pago = pago.MetodoPagoId,
                    monto = pago.Monto,
                    referencia_pago = pago.ReferenciaPago
                };

                _context.PagosFactura.Add(pagoFactura);
            }

            await _context.SaveChangesAsync();

            // 12. Actualizar stock de productos
            await ActualizarStockProductosAsync(facturaDto.Detalles);

            // 13. Actualizar consecutivo
            consecutivo.consecutivo_actual = siguienteNumero;
            _context.consecutivos.Update(consecutivo);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            // 14. Obtener la factura completa creada
            var facturaCompleta = await ObtenerFacturaCompletaAsync(factura.factura_id);

            return facturaCompleta;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ActionResponse<FacturaCompletaDTO>
            {
                WasSuccess = false,
                Message = $"Error al crear la factura: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<FacturaCompletaDTO>> ObtenerFacturaCompletaAsync(int facturaId)
    {
        try
        {
            var factura = await _context.Facturas
                .Include(f => f.Movimiento)
                    .ThenInclude(m => m!.Tercero)
                .Include(f => f.Movimiento)
                    .ThenInclude(m => m!.Usuario)
                .Include(f => f.DetallesFactura)
                    .ThenInclude(d => d!.Producto)
                        .ThenInclude(p => p!.TarifaIVA)
                .Include(f => f.PagosFactura)
                    .ThenInclude(p => p!.MetodoPago)
                .FirstOrDefaultAsync(f => f.factura_id == facturaId);

            if (factura == null)
            {
                return new ActionResponse<FacturaCompletaDTO>
                {
                    WasSuccess = false,
                    Message = "Factura no encontrada"
                };
            }

            var facturaCompleta = new FacturaCompletaDTO
            {
                FacturaId = factura.factura_id,
                //MovimientoId = factura.FK_movimiento_id,
                NumeroDocumento = factura.Movimiento!.numero_documento,
                Fecha = factura.Movimiento.fecha,
                //TerceroId = factura.Movimiento.FK_tercero_id,
                NombreTercero = $"{factura.Movimiento.Tercero!.nombre} {factura.Movimiento.Tercero.apellido1}",
                //UsuarioId = factura.Movimiento.FK_usuario_id,
                NombreUsuario = factura.Movimiento.Usuario!.nombre_usuario,
                Observaciones = factura.Movimiento.observaciones,
                TotalBruto = factura.total_bruto,
                TotalImpu = factura.total_impu,
                TotalDescuentos = factura.total_descuentos,
                TotalNeto = factura.total_neto,
                Detalles = factura.DetallesFactura?.Select(d => new DetalleFacturaCompletaDTO
                {
                    DetalleId = d.detalle_id,
                    ProductoId = d.FK_producto_id,
                    CodigoProducto = d.Producto!.codigo_producto,
                    NombreProducto = d.Producto.nombre,
                    Cantidad = d.cantidad,
                    PrecioUnitario = d.precio_unitario,
                    PorcentajeIva = d.Producto?.TarifaIVA?.porcentaje ?? 0, // Asignar el porcentaje de IVA
                    DescuentoPorcentaje = d.descuento_porcentaje,
                    DescuentoValor = d.descuento_valor,
                    Subtotal = d.subtotal
                }).ToList() ?? new List<DetalleFacturaCompletaDTO>(),
                Pagos = factura.PagosFactura?.Select(p => new PagoFacturaCompletaDTO
                {
                    PagoId = p.pago_id,
                    MetodoPagoId = p.FK_id_metodo_pago,
                    NombreMetodoPago = p.MetodoPago!.metodo_pago,
                    Monto = p.monto,
                    ReferenciaPago = p.referencia_pago
                }).ToList() ?? new List<PagoFacturaCompletaDTO>()
            };

            return new ActionResponse<FacturaCompletaDTO>
            {
                WasSuccess = true,
                Result = facturaCompleta
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<FacturaCompletaDTO>
            {
                WasSuccess = false,
                Message = $"Error al obtener la factura: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<FacturaCompletaDTO>>> ObtenerFacturasConFiltrosAsync(FacturaFiltroDTO filtros)
    {
        try
        {
            var query = _context.Facturas
                .Include(f => f.Movimiento)
                    .ThenInclude(m => m!.Tercero)
                .Include(f => f.Movimiento)
                    .ThenInclude(m => m!.Usuario)
                .AsQueryable();

            // Aplicar filtros
            if (filtros.FechaInicio.HasValue)
                query = query.Where(f => f.Movimiento!.fecha >= filtros.FechaInicio.Value);

            if (filtros.FechaFin.HasValue)
                query = query.Where(f => f.Movimiento!.fecha <= filtros.FechaFin.Value);

            if (filtros.TerceroId.HasValue)
                query = query.Where(f => f.Movimiento!.FK_tercero_id == filtros.TerceroId.Value);

            if (filtros.UsuarioId.HasValue)
                query = query.Where(f => f.Movimiento!.FK_usuario_id == filtros.UsuarioId.Value);

            if (filtros.MontoMinimo.HasValue)
                query = query.Where(f => f.total_neto >= filtros.MontoMinimo.Value);

            if (filtros.MontoMaximo.HasValue)
                query = query.Where(f => f.total_neto <= filtros.MontoMaximo.Value);

            if (!string.IsNullOrEmpty(filtros.NumeroDocumento))
                query = query.Where(f => f.Movimiento!.numero_documento.Contains(filtros.NumeroDocumento));

            var facturas = await query.ToListAsync();

            var facturasDto = facturas.Select(f => new FacturaCompletaDTO
            {
                FacturaId = f.factura_id,
                NumeroDocumento = f.Movimiento!.numero_documento,
                Fecha = f.Movimiento.fecha,
                NombreTercero = $"{f.Movimiento.Tercero!.nombre} {f.Movimiento.Tercero.apellido1}",
                NombreUsuario = f.Movimiento.Usuario!.nombre_usuario,
                Observaciones = f.Movimiento.observaciones,
                TotalBruto = f.total_bruto,
                TotalImpu = f.total_impu,  // ✅ AGREGADO: Incluir impuestos
                TotalDescuentos = f.total_descuentos,
                TotalNeto = f.total_neto
            });

            return new ActionResponse<IEnumerable<FacturaCompletaDTO>>
            {
                WasSuccess = true,
                Result = facturasDto
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<IEnumerable<FacturaCompletaDTO>>
            {
                WasSuccess = false,
                Message = $"Error al obtener las facturas: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<bool>> ValidarStockProductosAsync(List<DetalleFacturaItemDTO> detalles)
    {
        try
        {
            foreach (var detalle in detalles)
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.producto_id == detalle.ProductoId);

                if (producto == null)
                {
                    return new ActionResponse<bool>
                    {
                        WasSuccess = false,
                        Message = $"El producto con ID {detalle.ProductoId} no existe"
                    };
                }

                if (producto.stock_actual < detalle.Cantidad)
                {
                    return new ActionResponse<bool>
                    {
                        WasSuccess = false,
                        Message = $"Stock insuficiente para el producto {producto.nombre}. Stock disponible: {producto.stock_actual}, solicitado: {detalle.Cantidad}"
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
                Message = $"Error al validar stock: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<bool>> ActualizarStockProductosAsync(List<DetalleFacturaItemDTO> detalles)
    {
        try
        {
            foreach (var detalle in detalles)
            {
                var producto = await _context.Productos
                    .FirstOrDefaultAsync(p => p.producto_id == detalle.ProductoId);

                if (producto != null)
                {
                    producto.stock_actual -= detalle.Cantidad;
                    _context.Productos.Update(producto);
                }
            }

            await _context.SaveChangesAsync();

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
                Message = $"Error al actualizar stock: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<(decimal totalBruto, decimal totalDescuentos, decimal totalImpuestos, decimal totalNeto)>> CalcularTotalesFacturaAsync(List<DetalleFacturaItemDTO> detalles)
    {
        try
        {
            decimal totalBruto = 0;
            decimal totalDescuentos = 0;
            decimal totalImpuestos = 0;

            foreach (var detalle in detalles)
            {
                // Obtener el producto con su tarifa de IVA
                var producto = await _context.Productos
                    .Include(p => p.TarifaIVA)
                    .FirstOrDefaultAsync(p => p.producto_id == detalle.ProductoId);

                if (producto == null)
                {
                    return new ActionResponse<(decimal, decimal, decimal, decimal)>
                    {
                        WasSuccess = false,
                        Message = $"Producto con ID {detalle.ProductoId} no encontrado"
                    };
                }

                var porcentajeIva = producto.TarifaIVA?.porcentaje ?? 0;

                // Calcular valores del detalle
                var calculos = CalcularValoresDetalle(
                    detalle.Cantidad,
                    detalle.PrecioUnitario,
                    detalle.DescuentoPorcentaje,
                    detalle.DescuentoValor,
                    porcentajeIva
                );

                totalBruto += calculos.SubtotalBruto;
                totalDescuentos += calculos.DescuentoValor;
                totalImpuestos += calculos.ValorIva;
            }

            var totalNeto = totalBruto - totalDescuentos + totalImpuestos;

            return new ActionResponse<(decimal, decimal, decimal, decimal)>
            {
                WasSuccess = true,
                Result = (totalBruto, totalDescuentos, totalImpuestos, totalNeto)
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<(decimal, decimal, decimal, decimal)>
            {
                WasSuccess = false,
                Message = $"Error al calcular totales: {ex.Message}"
            };
        }
    }

    /// <summary>
    /// Calcula los valores de un detalle de factura incluyendo descuentos e IVA
    /// </summary>
    /// <param name="cantidad">Cantidad del producto</param>
    /// <param name="precioUnitario">Precio unitario del producto</param>
    /// <param name="descuentoPorcentaje">Porcentaje de descuento</param>
    /// <param name="descuentoValor">Valor del descuento (si se proporciona directamente)</param>
    /// <param name="porcentajeIva">Porcentaje de IVA del producto</param>
    /// <returns>Tupla con los valores calculados</returns>
    private (decimal SubtotalBruto, decimal DescuentoValor, decimal BaseGravable, decimal ValorIva, decimal Subtotal) CalcularValoresDetalle(
        int cantidad,
        decimal precioUnitario,
        decimal descuentoPorcentaje,
        decimal descuentoValor,
        decimal porcentajeIva)
    {
        // 1. Calcular subtotal bruto (sin descuentos ni impuestos)
        var subtotalBruto = cantidad * precioUnitario;

        // 2. Calcular el descuento_valor si solo se proporcionó el porcentaje
        if (descuentoValor == 0 && descuentoPorcentaje > 0)
        {
            descuentoValor = Math.Round(subtotalBruto * (descuentoPorcentaje / 100m), 2);
        }

        // 3. Calcular base gravable (subtotal bruto - descuentos)
        var baseGravable = subtotalBruto - descuentoValor;

        // 4. Calcular el valor del IVA sobre la base gravable
        var valorIva = Math.Round(baseGravable * (porcentajeIva / 100m), 2);

        // 5. Calcular subtotal final (base gravable + IVA)
        var subtotal = baseGravable + valorIva;

        return (
            Math.Round(subtotalBruto, 2),
            Math.Round(descuentoValor, 2),
            Math.Round(baseGravable, 2),
            Math.Round(valorIva, 2),
            Math.Round(subtotal, 2)
        );
    }

    public async Task<ActionResponse<bool>> AnularFacturaAsync(int facturaId, int usuarioId, string motivo)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var factura = await _context.Facturas
                .Include(f => f.DetallesFactura)
                .FirstOrDefaultAsync(f => f.factura_id == facturaId);

            if (factura == null)
            {
                return new ActionResponse<bool>
                {
                    WasSuccess = false,
                    Message = "Factura no encontrada"
                };
            }

            // Restaurar stock de productos
            if (factura.DetallesFactura != null)
            {
                foreach (var detalle in factura.DetallesFactura)
                {
                    var producto = await _context.Productos
                        .FirstOrDefaultAsync(p => p.producto_id == detalle.FK_producto_id);

                    if (producto != null)
                    {
                        producto.stock_actual += detalle.cantidad;
                        _context.Productos.Update(producto);
                    }
                }
            }

            // Marcar movimiento como anulado (agregar campo si no existe)
            var movimiento = await _context.Movimientos
                .FirstOrDefaultAsync(m => m.movimiento_id == factura.FK_movimiento_id);

            if (movimiento != null)
            {
                movimiento.observaciones = $"ANULADA - {motivo} - Usuario: {usuarioId} - Fecha: {DateTime.Now}";
                _context.Movimientos.Update(movimiento);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return new ActionResponse<bool>
            {
                WasSuccess = true,
                Result = true
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            return new ActionResponse<bool>
            {
                WasSuccess = false,
                Message = $"Error al anular factura: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<ResumenVentasDTO>> ObtenerResumenVentasAsync(DateTime fecha)
    {
        try
        {
            var facturas = await _context.Facturas
                .Include(f => f.Movimiento)
                .Include(f => f.PagosFactura)
                .ThenInclude(p => p!.MetodoPago)
                .Where(f => f.Movimiento!.fecha.Date == fecha.Date)
                .ToListAsync();

            var resumen = new ResumenVentasDTO
            {
                Fecha = fecha,
                TotalFacturas = facturas.Count,
                TotalVentas = facturas.Sum(f => f.total_bruto),
                TotalDescuentos = facturas.Sum(f => f.total_descuentos),
                VentaNeta = facturas.Sum(f => f.total_neto),
                VentasPorMetodoPago = facturas
                    .SelectMany(f => f.PagosFactura ?? new List<Pago_Factura>())
                    .GroupBy(p => new { p.FK_id_metodo_pago, p.MetodoPago!.metodo_pago })
                    .Select(g => new VentaPorMetodoPagoDTO
                    {
                        MetodoPagoId = g.Key.FK_id_metodo_pago,
                        NombreMetodoPago = g.Key.metodo_pago,
                        TotalVentas = g.Sum(p => p.monto),
                        CantidadTransacciones = g.Count()
                    })
                    .ToList()
            };

            return new ActionResponse<ResumenVentasDTO>
            {
                WasSuccess = true,
                Result = resumen
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<ResumenVentasDTO>
            {
                WasSuccess = false,
                Message = $"Error al obtener resumen de ventas: {ex.Message}"
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<FacturaCompletaDTO>>> ObtenerFacturasPendientesPagoAsync()
    {
        try
        {
            // Esta implementación asume que una factura está pendiente si no tiene pagos
            // o si la suma de pagos es menor al total neto
            var facturas = await _context.Facturas
                .Include(f => f.Movimiento)
                    .ThenInclude(m => m!.Tercero)
                .Include(f => f.PagosFactura)
                .Where(f => !f.PagosFactura!.Any() ||
                           f.PagosFactura.Sum(p => p.monto) < f.total_neto)
                .ToListAsync();

            var facturasDto = facturas.Select(f => new FacturaCompletaDTO
            {
                FacturaId = f.factura_id,
                //MovimientoId = f.FK_movimiento_id,
                NumeroDocumento = f.Movimiento!.numero_documento,
                Fecha = f.Movimiento.fecha,
                //TerceroId = f.Movimiento.FK_tercero_id,
                NombreTercero = $"{f.Movimiento.Tercero!.nombre} {f.Movimiento.Tercero.apellido1}",
                TotalNeto = f.total_neto
            });

            return new ActionResponse<IEnumerable<FacturaCompletaDTO>>
            {
                WasSuccess = true,
                Result = facturasDto
            };
        }
        catch (Exception ex)
        {
            return new ActionResponse<IEnumerable<FacturaCompletaDTO>>
            {
                WasSuccess = false,
                Message = $"Error al obtener facturas pendientes: {ex.Message}"
            };
        }
    }

    //private async Task<bool> ActualizarConsecutivoAsync()
    //{
    //    try
    //    {
    //        var consecutivo = await _context.consecutivos
    //            .FirstOrDefaultAsync(c => c.cod_consecut == "FV");

    //        if (consecutivo != null)
    //        {
    //            consecutivo.consecutivo_actual++;
    //            _context.consecutivos.Update(consecutivo);
    //            await _context.SaveChangesAsync();
    //        }

    //        return true;
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}
}
