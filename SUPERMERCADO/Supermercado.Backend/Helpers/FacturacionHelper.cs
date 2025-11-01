using Supermercado.Shared.DTOs;

namespace Supermercado.Backend.Helpers;

/// <summary>
/// Helper para cálculos y validaciones de facturación
/// </summary>
public static class FacturacionHelper
{
    /// <summary>
    /// Calcula el subtotal de un detalle de factura
    /// </summary>
    /// <param name="cantidad">Cantidad del producto</param>
    /// <param name="precioUnitario">Precio unitario del producto</param>
    /// <param name="descuentoValor">Valor del descuento aplicado</param>
    /// <returns>Subtotal calculado</returns>
    public static decimal CalcularSubtotal(int cantidad, decimal precioUnitario, decimal descuentoValor = 0)
    {
        var subtotalBruto = cantidad * precioUnitario;
        return Math.Max(0, subtotalBruto - descuentoValor);
    }

    /// <summary>
    /// Calcula el descuento en valor basado en el porcentaje
    /// </summary>
    /// <param name="cantidad">Cantidad del producto</param>
    /// <param name="precioUnitario">Precio unitario del producto</param>
    /// <param name="descuentoPorcentaje">Porcentaje de descuento</param>
    /// <returns>Valor del descuento</returns>
    public static decimal CalcularDescuentoValor(int cantidad, decimal precioUnitario, decimal descuentoPorcentaje)
    {
        if (descuentoPorcentaje <= 0) return 0;
        
        var subtotalBruto = cantidad * precioUnitario;
        return Math.Round(subtotalBruto * (descuentoPorcentaje / 100), 2);
    }

    /// <summary>
    /// Calcula los totales de una factura
    /// </summary>
    /// <param name="detalles">Lista de detalles de la factura</param>
    /// <returns>Tupla con (totalBruto, totalDescuentos, totalNeto)</returns>
    public static (decimal totalBruto, decimal totalDescuentos, decimal totalNeto) CalcularTotalesFactura(
        IEnumerable<DetalleFacturaItemDTO> detalles)
    {
        decimal totalBruto = 0;
        decimal totalDescuentos = 0;

        foreach (var detalle in detalles)
        {
            var subtotalBruto = detalle.Cantidad * detalle.PrecioUnitario;
            totalBruto += subtotalBruto;
            
            // Si no hay descuento valor, calcularlo del porcentaje
            var descuentoValor = detalle.DescuentoValor > 0 
                ? detalle.DescuentoValor 
                : CalcularDescuentoValor(detalle.Cantidad, detalle.PrecioUnitario, detalle.DescuentoPorcentaje);
            
            totalDescuentos += descuentoValor;
        }

        var totalNeto = totalBruto - totalDescuentos;
        
        return (
            Math.Round(totalBruto, 2),
            Math.Round(totalDescuentos, 2),
            Math.Round(totalNeto, 2)
        );
    }

    /// <summary>
    /// Valida que los pagos cubran exactamente el total de la factura
    /// </summary>
    /// <param name="totalFactura">Total neto de la factura</param>
    /// <param name="pagos">Lista de pagos</param>
    /// <param name="tolerancia">Tolerancia permitida en centavos (default: 0.01)</param>
    /// <returns>True si los pagos son válidos</returns>
    public static bool ValidarPagos(decimal totalFactura, IEnumerable<PagoFacturaItemDTO> pagos, decimal tolerancia = 0.01m)
    {
        var totalPagos = pagos.Sum(p => p.Monto);
        return Math.Abs(totalFactura - totalPagos) <= tolerancia;
    }

    /// <summary>
    /// Genera el número de documento con formato
    /// </summary>
    /// <param name="prefijo">Prefijo del documento (ej: "FV")</param>
    /// <param name="consecutivo">Número consecutivo</param>
    /// <param name="longitud">Longitud total del número (default: 6)</param>
    /// <returns>Número de documento formateado</returns>
    public static string GenerarNumeroDocumento(string prefijo, int consecutivo, int longitud = 6)
    {
        return $"{prefijo}{consecutivo.ToString().PadLeft(longitud, '0')}";
    }

    /// <summary>
    /// Valida que un detalle de factura sea válido
    /// </summary>
    /// <param name="detalle">Detalle a validar</param>
    /// <returns>Lista de errores de validación</returns>
    public static List<string> ValidarDetalle(DetalleFacturaItemDTO detalle)
    {
        var errores = new List<string>();

        if (detalle.ProductoId <= 0)
            errores.Add("El ID del producto debe ser mayor a 0");

        if (detalle.Cantidad <= 0)
            errores.Add("La cantidad debe ser mayor a 0");

        if (detalle.PrecioUnitario <= 0)
            errores.Add("El precio unitario debe ser mayor a 0");

        if (detalle.DescuentoPorcentaje < 0 || detalle.DescuentoPorcentaje > 100)
            errores.Add("El descuento porcentaje debe estar entre 0 y 100");

        if (detalle.DescuentoValor < 0)
            errores.Add("El descuento valor no puede ser negativo");

        // Validar que el descuento no exceda el subtotal
        var subtotalBruto = detalle.Cantidad * detalle.PrecioUnitario;
        var descuentoTotal = Math.Max(detalle.DescuentoValor, 
            CalcularDescuentoValor(detalle.Cantidad, detalle.PrecioUnitario, detalle.DescuentoPorcentaje));

        if (descuentoTotal > subtotalBruto)
            errores.Add("El descuento no puede ser mayor al subtotal del producto");

        return errores;
    }

    /// <summary>
    /// Valida que un pago sea válido
    /// </summary>
    /// <param name="pago">Pago a validar</param>
    /// <returns>Lista de errores de validación</returns>
    public static List<string> ValidarPago(PagoFacturaItemDTO pago)
    {
        var errores = new List<string>();

        if (pago.MetodoPagoId <= 0)
            errores.Add("El ID del método de pago debe ser mayor a 0");

        if (pago.Monto <= 0)
            errores.Add("El monto del pago debe ser mayor a 0");

        return errores;
    }

    /// <summary>
    /// Calcula el IVA de un producto (si se requiere en el futuro)
    /// </summary>
    /// <param name="subtotal">Subtotal del producto</param>
    /// <param name="porcentajeIva">Porcentaje de IVA</param>
    /// <returns>Valor del IVA</returns>
    public static decimal CalcularIVA(decimal subtotal, decimal porcentajeIva)
    {
        return Math.Round(subtotal * (porcentajeIva / 100), 2);
    }

    /// <summary>
    /// Formatea un valor monetario para mostrar
    /// </summary>
    /// <param name="valor">Valor a formatear</param>
    /// <param name="simboloMoneda">Símbolo de la moneda (default: "$")</param>
    /// <returns>Valor formateado</returns>
    public static string FormatearMoneda(decimal valor, string simboloMoneda = "$")
    {
        return $"{simboloMoneda}{valor:N2}";
    }

    /// <summary>
    /// Valida el horario de facturación
    /// </summary>
    /// <param name="horaInicio">Hora de inicio permitida (default: 6)</param>
    /// <param name="horaFin">Hora de fin permitida (default: 22)</param>
    /// <returns>True si está en horario permitido</returns>
    public static bool ValidarHorarioFacturacion(int horaInicio = 6, int horaFin = 22)
    {
        var horaActual = DateTime.Now.Hour;
        return horaActual >= horaInicio && horaActual <= horaFin;
    }

    /// <summary>
    /// Calcula el cambio a devolver
    /// </summary>
    /// <param name="totalFactura">Total de la factura</param>
    /// <param name="totalPagado">Total pagado por el cliente</param>
    /// <returns>Cambio a devolver (0 si no hay cambio)</returns>
    public static decimal CalcularCambio(decimal totalFactura, decimal totalPagado)
    {
        var cambio = totalPagado - totalFactura;
        return Math.Max(0, cambio);
    }
}
