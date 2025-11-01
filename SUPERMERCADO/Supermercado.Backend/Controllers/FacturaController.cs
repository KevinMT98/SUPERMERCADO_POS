using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar las facturas
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FacturaController : ControllerBase
{
    private readonly IFacturaUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FacturaController(IFacturaUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las facturas
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FacturaDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<FacturaDTO>>(action.Result);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene una factura por su ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FacturaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<FacturaDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Crea una nueva factura
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FacturaDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] FacturaCreateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = _mapper.Map<Factura>(model);
        var action = await _unitOfWork.AddAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        var dto = _mapper.Map<FacturaDTO>(action.Result);
        return Created($"/api/factura/{dto.factura_id}", dto);
    }

    /// <summary>
    /// Actualiza una factura existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(FacturaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] FacturaUpdateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != model.factura_id)
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");

        var entity = _mapper.Map<Factura>(model);
        var action = await _unitOfWork.UpdateAsync(entity);
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dto = _mapper.Map<FacturaDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Elimina una factura
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
