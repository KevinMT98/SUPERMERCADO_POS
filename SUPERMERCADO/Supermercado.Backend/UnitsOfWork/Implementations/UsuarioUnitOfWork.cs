using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unidad de trabajo para operaciones de usuarios
/// </summary>
public class UsuarioUnitOfWork : IUsuarioUnitOfWork
{
    private readonly IUsuarioRepository _repository;

    public UsuarioUnitOfWork(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public async Task<ActionResponse<IEnumerable<Usuario>>> GetAsync()
        => await _repository.GetAsync();

    public async Task<ActionResponse<Usuario>> GetAsync(int id)
        => await _repository.GetAsync(id);

    public async Task<ActionResponse<Usuario>> AddAsync(Usuario usuario)
        => await _repository.AddAsync(usuario);

    public async Task<ActionResponse<Usuario>> UpdateAsync(Usuario usuario)
        => await _repository.UpdateAsync(usuario);

    public async Task<ActionResponse<Usuario>> DeleteAsync(int id)
        => await _repository.DeleteAsync(id);

    public async Task<bool> ExistsByNombreUsuarioAsync(string nombreUsuario, int? excludeId = null)
        => await _repository.ExistsByNombreUsuarioAsync(nombreUsuario, excludeId);

    public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
        => await _repository.ExistsByEmailAsync(email, excludeId);
}
