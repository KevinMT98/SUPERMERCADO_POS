using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar los roles
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolsController : ControllerBase
{
    private readonly IRolUnitOfWork _unitOfWork;

    public RolsController(IRolUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Obtiene todos los roles
    /// </summary>
    /// <returns>Lista de roles</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Rol>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        return Ok(action.Result);
    }

    /// <summary>
    /// Obtiene un rol por su ID
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <returns>Rol solicitado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Rol), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        return Ok(action.Result);
    }

    /// <summary>
    /// Crea un nuevo rol
    /// </summary>
    /// <param name="model">Datos del rol a crear</param>
    /// <returns>Rol creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Rol), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostAsync([FromBody] Rol model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        // Validar que no exista un rol con el mismo nombre
        if (await _unitOfWork.ExistsByNombreAsync(model.nombre))
            return BadRequest("El nombre del rol ya existe.");
        
        var action = await _unitOfWork.AddAsync(model);
        
        if (!action.WasSuccess) return BadRequest(action.Message);
        
        return Created($"/api/rols/{action.Result!.rol_id}", action.Result);
    }

    /// <summary>
    /// Actualiza un rol existente
    /// </summary>
    /// <param name="id">ID del rol a actualizar</param>
    /// <param name="model">Datos actualizados del rol</param>
    /// <returns>Rol actualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Rol), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] Rol model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        if (id != model.rol_id) 
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");
        
        // Validar que no exista otro rol con el mismo nombre
        if (await _unitOfWork.ExistsByNombreAsync(model.nombre, id))
            return BadRequest("El nombre del rol ya existe en otro registro.");
        
        var action = await _unitOfWork.UpdateAsync(model);
        
        if (!action.WasSuccess) return BadRequest(action.Message);
        
        return Ok(action.Result);
    }

    /// <summary>
    /// Elimina un rol
    /// </summary>
    /// <param name="id">ID del rol a eliminar</param>
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
    /// Obtiene un rol por su nombre
    /// </summary>
    /// <param name="nombre">Nombre del rol</param>
    /// <returns>Rol encontrado</returns>
    [HttpGet("nombre/{nombre}")]
    [ProducesResponseType(typeof(Rol), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByNombreAsync(string nombre)
    {
        var action = await _unitOfWork.GetByNombreAsync(nombre);
        if (!action.WasSuccess) return NotFound(action.Message);
        return Ok(action.Result);
    }
}
