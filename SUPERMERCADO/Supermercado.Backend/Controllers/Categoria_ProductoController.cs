using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar las categorías de productos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class Categoria_ProductoController : ControllerBase
{
    private readonly ICategoriaProductoUnitOfWork _unitOfWork;

    public Categoria_ProductoController(ICategoriaProductoUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obtiene todas las categorías de productos
    /// </summary>
    /// <returns>Lista de categorías</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Categoria_Producto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        return Ok(action.Result);
    }

    /// <summary>
    /// Obtiene una categoría por su ID
    /// </summary>
    /// <param name="id">ID de la categoría</param>
    /// <returns>Categoría solicitada</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Categoria_Producto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        return Ok(action.Result);
    }

    /// <summary>
    /// Crea una nueva categoría de producto
    /// </summary>
    /// <param name="model">Datos de la categoría a crear</param>
    /// <returns>Categoría creada</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Categoria_Producto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] Categoria_Producto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        // Validar que no exista una categoría con la misma descripción
        if (await _unitOfWork.ExistsByDescripcionAsync(model.descripcion))
            return BadRequest("La descripción de la categoría ya existe.");
        
        var action = await _unitOfWork.AddAsync(model);
        
        if (!action.WasSuccess) return BadRequest(action.Message);
        
        return Created($"/api/categoria_producto/{action.Result!.categoriaId}", action.Result);
    }

    /// <summary>
    /// Actualiza una categoría de producto existente
    /// </summary>
    /// <param name="id">ID de la categoría a actualizar</param>
    /// <param name="model">Datos actualizados de la categoría</param>
    /// <returns>Categoría actualizada</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Categoria_Producto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] Categoria_Producto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (id != model.categoriaId) 
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");
        
        // Validar que no exista otra categoría con la misma descripción
        if (await _unitOfWork.ExistsByDescripcionAsync(model.descripcion, id))
            return BadRequest("La descripción de la categoría ya existe en otro registro.");
        
        var action = await _unitOfWork.UpdateAsync(model);
        
        if (!action.WasSuccess) return BadRequest(action.Message);
        
        return Ok(action.Result);
    }

    /// <summary>
    /// Elimina una categoría de producto
    /// </summary>
    /// <param name="id">ID de la categoría a eliminar</param>
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

    /// <summary>
    /// Obtiene categorías que tienen productos asociados
    /// </summary>
    /// <returns>Lista de categorías con productos</returns>
    [HttpGet("con-productos")]
    [ProducesResponseType(typeof(IEnumerable<Categoria_Producto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCategoriasConProductosAsync()
    {
        var action = await _unitOfWork.GetCategoriasConProductosAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        return Ok(action.Result);
    }

    /// <summary>
    /// Obtiene solo las categorías activas
    /// </summary>
    /// <returns>Lista de categorías activas</returns>
    [HttpGet("activas")]
    [ProducesResponseType(typeof(IEnumerable<Categoria_Producto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCategoriasActivasAsync()
    {
        var action = await _unitOfWork.GetCategoriasActivasAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        return Ok(action.Result);
    }
}
