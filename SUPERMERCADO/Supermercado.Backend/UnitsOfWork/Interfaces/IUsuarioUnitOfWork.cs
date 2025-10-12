using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;

/// <summary>
/// Interfaz de unidad de trabajo para operaciones de usuarios
/// </summary>
public interface IUsuarioUnitOfWork
{
    Task<ActionResponse<IEnumerable<Usuario>>> GetAsync();
    Task<ActionResponse<Usuario>> GetAsync(int id);
    Task<ActionResponse<Usuario>> AddAsync(Usuario usuario);
    Task<ActionResponse<Usuario>> UpdateAsync(Usuario usuario);
    Task<ActionResponse<Usuario>> DeleteAsync(int id);
    Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario, int? excludeId = null);
    Task<bool> ExistsByEmailAsync(string email, int? excludeId = null);
}
