using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unidad de trabajo específica para Rol
/// Coordina operaciones del repositorio de Rol y expone métodos personalizados
/// </summary>
public class RolUnitOfWork : GenericUnitOfWork<Rol>, IRolUnitOfWork
{
    private readonly IRolRepository _rolRepository;

    public RolUnitOfWork(IRolRepository rolRepository) : base(rolRepository)
    {
        _rolRepository = rolRepository;
    }

    public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)
        => await _rolRepository.ExistsByNombreAsync(nombre, excludeId);

    public async Task<ActionResponse<Rol>> GetByNombreAsync(string nombre)
        => await _rolRepository.GetByNombreAsync(nombre);

    public async Task<ActionResponse<IEnumerable<Rol>>> GetRolesActivosAsync()
        => await _rolRepository.GetRolesActivosAsync();
}
