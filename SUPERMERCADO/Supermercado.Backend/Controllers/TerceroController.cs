using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TerceroController : ControllerBase
{
    private readonly ITerceroUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TerceroController(ITerceroUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TerceroDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<TerceroDTO>>(action.Result);
        return Ok(dtos);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TerceroDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<TerceroDTO>(action.Result);
        return Ok(dto);
    }

    [HttpGet("identificacion/{numeroIdentificacion}")]
    [ProducesResponseType(typeof(TerceroDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdentificacionAsync(string numeroIdentificacion)
    {
        var action = await _unitOfWork.GetByIdentificacionAsync(numeroIdentificacion);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<TerceroDTO>(action.Result);
        return Ok(dto);
    }

    [HttpGet("activos")]
    [ProducesResponseType(typeof(IEnumerable<TerceroDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTercerosActivosAsync()
    {
        var action = await _unitOfWork.GetTercerosActivosAsync();
        if (!action.WasSuccess) return NotFound(action.Message);
        var dtos = _mapper.Map<IEnumerable<TerceroDTO>>(action.Result);
        return Ok(dtos);
    }

    [HttpGet("clientes")]
    [ProducesResponseType(typeof(IEnumerable<TerceroDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTercerosClienteAsync()
    {
        var action = await _unitOfWork.GetTercerosClienteAsync();
        if (!action.WasSuccess) return NotFound(action.Message);
        var dtos = _mapper.Map<IEnumerable<TerceroDTO>>(action.Result);
        return Ok(dtos);
    }

    [HttpGet("proveedores")]
    [ProducesResponseType(typeof(IEnumerable<TerceroDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTercerosProveedorAsync()
    {
        var action = await _unitOfWork.GetTercerosProveedorAsync();
        if (!action.WasSuccess) return NotFound(action.Message);
        var dtos = _mapper.Map<IEnumerable<TerceroDTO>>(action.Result);
        return Ok(dtos);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TerceroDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] TerceroCreateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (await _unitOfWork.ExistsByIdentificacionAsync(model.numero_identificacion))
            return BadRequest("El número de identificación del tercero ya existe.");

        var entity = _mapper.Map<Tercero>(model);
        var action = await _unitOfWork.AddAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        var dto = _mapper.Map<TerceroDTO>(action.Result);
        return Created($"/api/tercero/{dto.tercero_id}", dto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TerceroDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] TerceroUpdateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != model.tercero_id)
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");

        // Validar que no exista otro tercero con el mismo número de identificación
        if (await _unitOfWork.ExistsByIdentificacionAsync(model.numero_identificacion, id))
            return BadRequest("El número de identificación ya existe en otro registro.");

        var entity = _mapper.Map<Tercero>(model);
        var action = await _unitOfWork.UpdateAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        return Ok(_mapper.Map<TerceroDTO>(action.Result));
    }

    [HttpPut("identificacion/{numeroIdentificacion}")]
    [ProducesResponseType(typeof(TerceroDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> PutByIdentificacionAsync(string numeroIdentificacion, [FromBody] TerceroUpdateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (numeroIdentificacion != model.numero_identificacion)
            return BadRequest("El número de identificación de la URL no coincide con el del modelo.");

        // Mapear DTO a entidad
        var entity = _mapper.Map<Tercero>(model);
        
        var action = await _unitOfWork.UpdateByIdentificacionAsync(numeroIdentificacion, entity);

        if (!action.WasSuccess) return NotFound(action.Message);

        return Ok(_mapper.Map<TerceroDTO>(action.Result));
    }

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

