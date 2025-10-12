using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar los productos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductoController : ControllerBase
{
    private readonly IProductoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductoController(IProductoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los productos
    /// </summary>
    /// <returns>Lista de productos</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProductoDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<ProductoDto>>(action.Result);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene un producto por su ID
    /// </summary>
    /// <param name="id">ID del producto</param>
    /// <returns>Producto solicitado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<ProductoDto>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Crea un nuevo producto
    /// </summary>
    /// <param name="model">Datos del producto a crear</param>
    /// <returns>Producto creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] ProductoCreateDto model)
    {

        if (await _unitOfWork.ExistsByCodigoProductoAsync(model.CodigoProducto))
            return BadRequest("El código de producto ya existe.");
        if (await _unitOfWork.ExistsByCodigoBarrasAsync(model.CodigoBarras))
            return BadRequest("El código de barras ya existe.");
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var entity = _mapper.Map<Producto>(model);
        var action = await _unitOfWork.AddAsync(entity);
        
        if (!action.WasSuccess) return BadRequest(action.Message);
        
        var dto = _mapper.Map<ProductoDto>(action.Result);
        return Created($"/api/producto/{dto.ProductoId}", dto);
    }

    /// <summary>
    /// Actualiza un producto existente
    /// </summary>
    /// <param name="id">ID del producto a actualizar</param>
    /// <param name="model">Datos actualizados del producto</param>
    /// <returns>Producto actualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ProductoDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] ProductoUpdateDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (id != model.ProductoId) 
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");
        
        // Validar que no exista otro producto con el mismo código de producto
        if (await _unitOfWork.ExistsByCodigoProductoAsync(model.CodigoProducto, id))
            return BadRequest("El código de producto ya existe en otro registro.");
        
        // Validar que no exista otro producto con el mismo código de barras
        if (await _unitOfWork.ExistsByCodigoBarrasAsync(model.CodigoBarras, id))
            return BadRequest("El código de barras ya existe en otro registro.");
        
        var entity = _mapper.Map<Producto>(model);
        var action = await _unitOfWork.UpdateAsync(entity);
        
        if (!action.WasSuccess) return BadRequest(action.Message);
        
        return Ok(_mapper.Map<ProductoDto>(action.Result));
    }

    /// <summary>
    /// Elimina un producto
    /// </summary>
    /// <param name="id">ID del producto a eliminar</param>
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
