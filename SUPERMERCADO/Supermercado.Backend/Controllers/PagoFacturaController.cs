using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar los pagos de factura
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PagoFacturaController : ControllerBase
{
    private readonly IPagoFacturaUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PagoFacturaController(IPagoFacturaUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los pagos de factura
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PagoFacturaDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<PagoFacturaDTO>>(action.Result);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene un pago de factura por su ID
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PagoFacturaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<PagoFacturaDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Crea un nuevo pago de factura
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(PagoFacturaDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] PagoFacturaCreateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = _mapper.Map<Pago_Factura>(model);
        var action = await _unitOfWork.AddAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        var dto = _mapper.Map<PagoFacturaDTO>(action.Result);
        return Created($"/api/pagofactura/{dto.pago_id}", dto);
    }

    /// <summary>
    /// Actualiza un pago de factura existente
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PagoFacturaDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] PagoFacturaUpdateDTO model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        if (id != model.pago_id)
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");

        var entity = _mapper.Map<Pago_Factura>(model);
        var action = await _unitOfWork.UpdateAsync(entity);
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dto = _mapper.Map<PagoFacturaDTO>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Elimina un pago de factura
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
