using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar los métodos de pago
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MetodosPagoController : ControllerBase
{
    private readonly IMetodosPagoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MetodosPagoController(IMetodosPagoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los métodos de pago
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MetodosPagoDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<MetodosPagoDTO>>(action.Result);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene un método de pago por su ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MetodosPagoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<MetodosPagoDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Crea un nuevo método de pago
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(MetodosPagoDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] MetodosPagoCreateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = _mapper.Map<Metodos_Pago>(model);
        var action = await _unitOfWork.AddAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        var dto = _mapper.Map<MetodosPagoDTO>(action.Result);
        return Created($"/api/metodospago/{dto.id_metodo_pago}", dto);
    }

    /// <summary>
    /// Actualiza un método de pago existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MetodosPagoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] MetodosPagoUpdateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != model.id_metodo_pago)
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");

        var entity = _mapper.Map<Metodos_Pago>(model);
        var action = await _unitOfWork.UpdateAsync(entity);
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dto = _mapper.Map<MetodosPagoDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Elimina un método de pago
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
