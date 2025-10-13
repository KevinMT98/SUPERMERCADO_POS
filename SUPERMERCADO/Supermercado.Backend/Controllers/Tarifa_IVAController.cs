using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar las tarifas de IVA
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TarifaIvaController : ControllerBase
{
    private readonly ITarifaIvaUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TarifaIvaController(ITarifaIvaUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las tarifas de IVA
    /// </summary>
    /// <returns>Lista de tarifas de IVA</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TarifaIvaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<TarifaIvaDto>>(action.Result);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene solo las tarifas de IVA activas
    /// </summary>
    /// <returns>Lista de tarifas de IVA activas</returns>
    [HttpGet("activas")]
    [ProducesResponseType(typeof(IEnumerable<TarifaIvaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetTarifasActivasAsync()
    {
        var action = await _unitOfWork.GetTarifasActivasAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<TarifaIvaDto>>(action.Result);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene una tarifa de IVA por su ID
    /// </summary>
    /// <param name="id">ID de la tarifa de IVA</param>
    /// <returns>Tarifa de IVA solicitada</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TarifaIvaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<TarifaIvaDto>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Crea una nueva tarifa de IVA
    /// </summary>
    /// <param name="model">Datos de la tarifa de IVA a crear</param>
    /// <returns>Tarifa de IVA creada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TarifaIvaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] TarifaIvaCreateDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        // Validar que no exista una tarifa con el mismo código IVA
        if (await _unitOfWork.ExistsByCodigoIvaAsync(model.CodigoIva))
            return BadRequest("El código IVA ya existe.");
        
        var entity = _mapper.Map<Tarifa_IVA>(model);
        var action = await _unitOfWork.AddAsync(entity);
        
        if (!action.WasSuccess) return BadRequest(action.Message);
        
        var dto = _mapper.Map<TarifaIvaDto>(action.Result);
        return Created($"/api/tarifaiva/{dto.TarifaIvaId}", dto);
    }

    /// <summary>
    /// Actualiza una tarifa de IVA existente
    /// </summary>
    /// <param name="id">ID de la tarifa de IVA a actualizar</param>
    /// <param name="model">Datos actualizados de la tarifa de IVA</param>
    /// <returns>Tarifa de IVA actualizada</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TarifaIvaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] TarifaIvaUpdateDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (id != model.TarifaIvaId) 
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");
        
        // Validar que no exista otra tarifa con el mismo código IVA
        if (await _unitOfWork.ExistsByCodigoIvaAsync(model.CodigoIva, id))
            return BadRequest("El código IVA ya existe en otro registro.");
        
        var entity = _mapper.Map<Tarifa_IVA>(model);
        var action = await _unitOfWork.UpdateAsync(entity);
        
        if (!action.WasSuccess) return BadRequest(action.Message);
        
        return Ok(_mapper.Map<TarifaIvaDto>(action.Result));
    }

    /// <summary>
    /// Elimina una tarifa de IVA
    /// </summary>
    /// <param name="id">ID de la tarifa de IVA a eliminar</param>
    /// <returns>Sin contenido si se eliminó correctamente</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var action = await _unitOfWork.DeleteAsync(id);
        if (!action.WasSuccess) return BadRequest(action.Message);
        return NoContent();
    }
}

