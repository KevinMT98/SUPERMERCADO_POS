using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;
namespace Supermercado.Backend.UnitsOfWork.Implementations;

public class TiposIdentificacionUnitOfWork : GenericUnitOfWork<TiposIdentificacion>, ITiposIdentificacionUnitOfWork
{
    private readonly ITiposIdentificacionRepository _tiposIdentidadRepository;
    public TiposIdentificacionUnitOfWork(ITiposIdentificacionRepository tiposIdentidadRepository) 
        : base(tiposIdentidadRepository)
    {
        _tiposIdentidadRepository = tiposIdentidadRepository;
    }
    public async Task<bool> ExistsByDescripcionAsync(string descripcion, int? excludeId = null)
        => await _tiposIdentidadRepository.ExistsByDescripcionAsync(descripcion, excludeId);
    public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null)
        => await _tiposIdentidadRepository.ExistsByCodigoAsync(codigo, excludeId);
    public async Task<ActionResponse<TiposIdentificacion>> GetByDescripcionAsync(string descripcion)
        => await _tiposIdentidadRepository.GetByDescripcionAsync(descripcion);


}
