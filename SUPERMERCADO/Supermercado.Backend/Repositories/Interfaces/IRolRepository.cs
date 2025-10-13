using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Rol con validaciones personalizadas
/// </summary>
public interface IRolRepository : IGenericRepository<Rol>
{
    /// <summary>
    /// Verifica si existe un rol con el nombre especificado
    /// </summary>
    /// <param name="nombre">Nombre del rol a verificar</param>
    /// <param name="excludeId">ID del rol a excluir de la búsqueda (útil para actualizaciones)</param>
    /// <returns>True si existe, False si no existe</returns>
    Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null);

    /// <summary>
    /// Obtiene un rol por su nombre
    /// </summary>
    /// <param name="nombre">Nombre del rol a buscar</param>
    /// <returns>Rol encontrado</returns>
    Task<ActionResponse<Rol>> GetByNombreAsync(string nombre);

    Task<ActionResponse<IEnumerable<Rol>>> GetRolesActivosAsync();
}
