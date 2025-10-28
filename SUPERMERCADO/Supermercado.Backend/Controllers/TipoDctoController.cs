using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar los roles
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TipoDctoController : ControllerBase
{

    private readonly ITipoDctoUnitOfWork _unitOfWork;

    public TipoDctoController(ITipoDctoUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TipoDcto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        return Ok(action.Result);
    }


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TipoDcto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        return Ok(action.Result);
    }


    [HttpPost]
    [ProducesResponseType(typeof(TipoDcto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] TipoDcto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var action = await _unitOfWork.AddAsync(model);

        if (!action.WasSuccess) return BadRequest(action.Message);

        return Created($"/api/tipodcto/{action.Result!.ID}", action.Result);
    }


    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TipoDcto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] TipoDcto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != model.ID)
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");

        // Validar que no exista otra categoría con la misma descripción

        var action = await _unitOfWork.UpdateAsync(model);

        if (!action.WasSuccess) return BadRequest(action.Message);

        return Ok(action.Result);
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
