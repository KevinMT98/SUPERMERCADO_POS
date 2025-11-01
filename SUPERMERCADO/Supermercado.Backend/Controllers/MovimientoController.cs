using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar los movimientos de inventario y documentos
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MovimientoController : ControllerBase
{
    private readonly IMovimientoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MovimientoController(IMovimientoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los movimientos
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MovimientoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<MovimientoDTO>>(action.Result);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene un movimiento por su ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MovimientoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<MovimientoDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Crea un nuevo movimiento
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MovimientoDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] MovimientoCreateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = _mapper.Map<Movimiento>(model);
        var action = await _unitOfWork.AddAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        var dto = _mapper.Map<MovimientoDTO>(action.Result);
        return Created($"/api/movimiento/{dto.movimiento_id}", dto);
    }

    /// <summary>
    /// Actualiza un movimiento existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MovimientoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] MovimientoUpdateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != model.movimiento_id)
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");

        var entity = _mapper.Map<Movimiento>(model);
        var action = await _unitOfWork.UpdateAsync(entity);
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dto = _mapper.Map<MovimientoDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Elimina un movimiento
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
