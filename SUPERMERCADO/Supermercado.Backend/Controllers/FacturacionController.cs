using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using System.Security.Claims;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para el proceso completo de facturación
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class FacturacionController : ControllerBase
{
    private readonly IFacturacionUnitOfWork _facturacionUnitOfWork;

    public FacturacionController(IFacturacionUnitOfWork facturacionUnitOfWork)
    {
        _facturacionUnitOfWork = facturacionUnitOfWork;
    }

    /// <summary>
    /// Crea una factura completa con detalles y pagos
    /// </summary>
    /// <param name="facturaDto">Datos de la factura a crear</param>
    /// <returns>Factura creada con todos sus detalles</returns>
    [HttpPost("crear-factura")]
    [ProducesResponseType(typeof(FacturaCompletaDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CrearFacturaCompletaAsync([FromBody] FacturaCompletaCreateDTO facturaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // Obtener el usuario actual del token JWT
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out int usuarioId))
        {
            return Unauthorized("No se pudo identificar el usuario");
        }

        // Asignar el usuario actual si no se especificó
        if (facturaDto.UsuarioId == 0)
        {
            facturaDto.UsuarioId = usuarioId;
        }

        var resultado = await _facturacionUnitOfWork.CrearFacturaCompletaAsync(facturaDto);

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Created($"/api/facturacion/{resultado.Result!.FacturaId}", resultado.Result);
    }

    /// <summary>
    /// Obtiene una factura completa por su ID
    /// </summary>
    /// <param name="id">ID de la factura</param>
    /// <returns>Factura con todos sus detalles</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FacturaCompletaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObtenerFacturaCompletaAsync(int id)
    {
        var resultado = await _facturacionUnitOfWork.ObtenerFacturaCompletaAsync(id);

        if (!resultado.WasSuccess)
        {
            return NotFound(resultado.Message);
        }

        return Ok(resultado.Result);
    }

    /// <summary>
    /// Obtiene facturas con filtros aplicados
    /// </summary>
    /// <param name="filtros">Filtros a aplicar</param>
    /// <returns>Lista de facturas que cumplen los filtros</returns>
    [HttpPost("buscar")]
    [ProducesResponseType(typeof(IEnumerable<FacturaCompletaDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BuscarFacturasAsync([FromBody] FacturaFiltroDTO filtros)
    {
        var resultado = await _facturacionUnitOfWork.ObtenerFacturasConFiltrosAsync(filtros);

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok(resultado.Result);
    }

    /// <summary>
    /// Anula una factura existente
    /// </summary>
    /// <param name="id">ID de la factura a anular</param>
    /// <param name="motivo">Motivo de la anulación</param>
    /// <returns>Confirmación de anulación</returns>
    [HttpPut("{id:int}/anular")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AnularFacturaAsync(int id, [FromBody] string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo))
        {
            return BadRequest("El motivo de anulación es obligatorio");
        }

        // Obtener el usuario actual del token JWT
        var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(usuarioIdClaim) || !int.TryParse(usuarioIdClaim, out int usuarioId))
        {
            return Unauthorized("No se pudo identificar el usuario");
        }

        var resultado = await _facturacionUnitOfWork.AnularFacturaAsync(id, usuarioId, motivo);

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok(new { message = "Factura anulada exitosamente", facturaId = id });
    }

    /// <summary>
    /// Obtiene el resumen de ventas por fecha
    /// </summary>
    /// <param name="fecha">Fecha para el resumen (formato: yyyy-MM-dd)</param>
    /// <returns>Resumen de ventas del día</returns>
    [HttpGet("resumen-ventas/{fecha:datetime}")]
    [ProducesResponseType(typeof(ResumenVentasDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ObtenerResumenVentasAsync(DateTime fecha)
    {
        var resultado = await _facturacionUnitOfWork.ObtenerResumenVentasAsync(fecha);

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok(resultado.Result);
    }

    /// <summary>
    /// Obtiene facturas pendientes de pago
    /// </summary>
    /// <returns>Lista de facturas pendientes</returns>
    [HttpGet("pendientes-pago")]
    [ProducesResponseType(typeof(IEnumerable<FacturaCompletaDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerFacturasPendientesPagoAsync()
    {
        var resultado = await _facturacionUnitOfWork.ObtenerFacturasPendientesPagoAsync();

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok(resultado.Result);
    }

    /// <summary>
    /// Obtiene productos disponibles para facturación
    /// </summary>
    /// <returns>Lista de productos con stock disponible</returns>
    [HttpGet("productos-disponibles")]
    [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerProductosDisponiblesAsync()
    {
        var resultado = await _facturacionUnitOfWork.ObtenerProductosDisponiblesAsync();

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok(resultado.Result);
    }

    /// <summary>
    /// Obtiene métodos de pago activos
    /// </summary>
    /// <returns>Lista de métodos de pago disponibles</returns>
    [HttpGet("metodos-pago")]
    [ProducesResponseType(typeof(IEnumerable<MetodosPagoDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerMetodosPagoActivosAsync()
    {
        var resultado = await _facturacionUnitOfWork.ObtenerMetodosPagoActivosAsync();

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok(resultado.Result);
    }

    /// <summary>
    /// Obtiene clientes activos
    /// </summary>
    /// <returns>Lista de clientes disponibles para facturación</returns>
    [HttpGet("clientes")]
    [ProducesResponseType(typeof(IEnumerable<TerceroDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerClientesActivosAsync()
    {
        var resultado = await _facturacionUnitOfWork.ObtenerClientesActivosAsync();

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok(resultado.Result);
    }

    /// <summary>
    /// Valida los datos de una factura antes de crearla
    /// </summary>
    /// <param name="facturaDto">Datos de la factura a validar</param>
    /// <returns>Resultado de la validación</returns>
    [HttpPost("validar")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ValidarDatosFacturaAsync([FromBody] FacturaCompletaCreateDTO facturaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var resultado = await _facturacionUnitOfWork.ValidarDatosFacturaAsync(facturaDto);

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok(new { message = "Datos de factura válidos", esValida = true });
    }

    /// <summary>
    /// Obtiene facturas del día actual
    /// </summary>
    /// <returns>Lista de facturas del día</returns>
    [HttpGet("hoy")]
    [ProducesResponseType(typeof(IEnumerable<FacturaCompletaDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerFacturasHoyAsync()
    {
        var filtros = new FacturaFiltroDTO
        {
            FechaInicio = DateTime.Today,
            FechaFin = DateTime.Today.AddDays(1).AddTicks(-1)
        };

        var resultado = await _facturacionUnitOfWork.ObtenerFacturasConFiltrosAsync(filtros);

        if (!resultado.WasSuccess)
        {
            return BadRequest(resultado.Message);
        }

        return Ok(resultado.Result);
    }

    /// <summary>
    /// Obtiene estadísticas rápidas de facturación
    /// </summary>
    /// <returns>Estadísticas del día actual</returns>
    [HttpGet("estadisticas")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ObtenerEstadisticasAsync()
    {
        var resumenHoy = await _facturacionUnitOfWork.ObtenerResumenVentasAsync(DateTime.Today);
        var facturasPendientes = await _facturacionUnitOfWork.ObtenerFacturasPendientesPagoAsync();

        var estadisticas = new
        {
            VentasHoy = resumenHoy.WasSuccess ? resumenHoy.Result : null,
            FacturasPendientes = facturasPendientes.WasSuccess ? facturasPendientes.Result?.Count() ?? 0 : 0,
            FechaConsulta = DateTime.Now
        };

        return Ok(estadisticas);
    }
}
