using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.DTOs;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Controllers;

/// <summary>
/// Controlador para gestionar el maestro de usuarios (CRUD de usuarios)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")] // Solo administradores pueden gestionar usuarios
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UsuarioController(IUsuarioUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los usuarios del sistema
    /// </summary>
    /// <returns>Lista de usuarios</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        if (!action.WasSuccess) return BadRequest(action.Message);
        var dtos = _mapper.Map<IEnumerable<UsuarioDto>>(action.Result);
        return Ok(dtos);
    }

    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Usuario solicitado</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetAsync(int id)
    {
        var action = await _unitOfWork.GetAsync(id);
        if (!action.WasSuccess) return NotFound(action.Message);
        var dto = _mapper.Map<UsuarioDto>(action.Result);
        return Ok(dto);
    }

    /// <summary>
    /// Crea un nuevo usuario en el sistema
    /// </summary>
    /// <param name="model">Datos del nuevo usuario</param>
    /// <returns>Usuario creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Usuario), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PostAsync([FromBody] UsuarioCreateDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Validar que el nombre de usuario no exista
        if (await _unitOfWork.ExistsByNombreUsuarioAsync(model.NombreUsuario))
            return BadRequest("El nombre de usuario ya está en uso.");

        // Validar que el email no exista (si se proporciona)
        if (!string.IsNullOrEmpty(model.Email) && await _unitOfWork.ExistsByEmailAsync(model.Email))
            return BadRequest("El email ya está en uso.");

        var entity = _mapper.Map<Usuario>(model);
        var action = await _unitOfWork.AddAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);
        var dto = _mapper.Map<UsuarioDto>(action.Result);
        return Created($"/api/usuario/{dto.UsuarioId}", dto);
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    /// <param name="id">ID del usuario a actualizar</param>
    /// <param name="model">Datos actualizados del usuario</param>
    /// <returns>Usuario actualizado</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> PutAsync(int id, [FromBody] UsuarioUpdateDto model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        if (id != model.UsuarioId)
            return BadRequest("El ID de la URL no coincide con el ID del modelo.");

        // Validar que el nombre de usuario no exista en otro usuario
        if (await _unitOfWork.ExistsByNombreUsuarioAsync(model.NombreUsuario, id))
            return BadRequest("El nombre de usuario ya está en uso por otro usuario.");

        // Validar que el email no exista en otro usuario (si se proporciona)
        if (!string.IsNullOrEmpty(model.Email) && await _unitOfWork.ExistsByEmailAsync(model.Email, id))
            return BadRequest("El email ya está en uso por otro usuario.");

        var entity = _mapper.Map<Usuario>(model);
        var action = await _unitOfWork.UpdateAsync(entity);

        if (!action.WasSuccess) return BadRequest(action.Message);

        return Ok(_mapper.Map<UsuarioDto>(action.Result));
    }

    /// <summary>
    /// Elimina un usuario del sistema
    /// </summary>
    /// <param name="id">ID del usuario a eliminar</param>
    /// <returns>Sin contenido si se eliminó correctamente</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var action = await _unitOfWork.DeleteAsync(id);
        if (!action.WasSuccess) return BadRequest(action.Message);
        return NoContent();
    }

    /// <summary>
    /// Activa o desactiva un usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="activo">Estado (true = activo, false = inactivo)</param>
    /// <returns>Usuario actualizado</returns>
    [HttpPatch("{id}/estado")]
    [ProducesResponseType(typeof(Usuario), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CambiarEstadoAsync(int id, [FromBody] bool activo)
    {
        var userResponse = await _unitOfWork.GetAsync(id);
        if (!userResponse.WasSuccess) return NotFound(userResponse.Message);

        var usuario = userResponse.Result!;
        usuario.activo = activo;

        var action = await _unitOfWork.UpdateAsync(usuario);
        if (!action.WasSuccess) return BadRequest(action.Message);

        return Ok(action.Result);
    }
}
