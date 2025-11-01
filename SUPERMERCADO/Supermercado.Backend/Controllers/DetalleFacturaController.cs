using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar los detalles de factura
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DetalleFacturaController : ControllerBase
{
    private readonly IDetalleFacturaUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DetalleFacturaController(IDetalleFacturaUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los detalles de factura
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<DetalleFacturaDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<DetalleFacturaDTO>>(action.Result);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene un detalle de factura por su ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DetalleFacturaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<DetalleFacturaDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Crea un nuevo detalle de factura
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(DetalleFacturaDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] DetalleFacturaCreateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = _mapper.Map<Detalle_Factura>(model);
        var action = await _unitOfWork.AddAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        var dto = _mapper.Map<DetalleFacturaDTO>(action.Result);
        return Created($"/api/detallefactura/{dto.detalle_id}", dto);
    }

    /// <summary>
    /// Actualiza un detalle de factura existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(DetalleFacturaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] DetalleFacturaUpdateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != model.detalle_id)
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");

        var entity = _mapper.Map<Detalle_Factura>(model);
        var action = await _unitOfWork.UpdateAsync(entity);
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dto = _mapper.Map<DetalleFacturaDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Elimina un detalle de factura
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var action = await _unitOfWork.DeleteAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        return NoContent();
    }
}
