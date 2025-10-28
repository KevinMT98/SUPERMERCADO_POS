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
public class TiposIdentificacionController : ControllerBase
{
    private readonly ITiposIdentificacionUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TiposIdentificacionController(ITiposIdentificacionUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TiposIdentificacionDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<TiposIdentificacionDTO>>(action.Result);
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TiposIdentificacionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<TiposIdentificacionDTO>(action.Result);
        return Ok(dto);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TiposIdentificacionDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] TiposIdentificacionCreateDTO model)
    {

        if (await _unitOfWork.ExistsByCodigoAsync(model.tipoDocumentoID))
            return BadRequest("El código de producto ya existe.");
        if (await _unitOfWork.ExistsByDescripcionAsync(model.descripcion))
            return BadRequest("La descripcion del tipo de identificacion ya existe");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = _mapper.Map<TiposIdentificacion>(model);
        var action = await _unitOfWork.AddAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        var dto = _mapper.Map<TiposIdentificacion>(action.Result);
        return Created($"/api/tiposidentificacion/{dto.tipoDocumentoID}", dto);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TiposIdentificacion), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] TiposIdentificacionUpdateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != model.ID)
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");

        // Validar que no exista otro registro con el mismo código de tipoDocumentoID
        if (await _unitOfWork.ExistsByCodigoAsync(model.tipoDocumentoID, id))
            return BadRequest("El código de tipo de identificación ya existe en otro registro.");

        // Validar que no exista otro registro con la misma descripción
        if (await _unitOfWork.ExistsByDescripcionAsync(model.descripcion, id))
            return BadRequest("La descripción ya existe en otro registro.");

        var entity = _mapper.Map<TiposIdentificacion>(model);
        var action = await _unitOfWork.UpdateAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        return Ok(_mapper.Map<TiposIdentificacionDTO>(action.Result));
    }


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
