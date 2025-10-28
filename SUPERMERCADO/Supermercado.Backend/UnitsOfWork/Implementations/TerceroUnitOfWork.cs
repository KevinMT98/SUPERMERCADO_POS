using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

public class TerceroUnitOfWork : GenericUnitOfWork<Tercero>, ITerceroUnitOfWork
{
    private readonly ITerceroRepository _terceroRepository;

    public TerceroUnitOfWork(ITerceroRepository terceroRepository) : base(terceroRepository)
    {
        _terceroRepository = terceroRepository;
    }

    public async Task<bool> ExistsByIdentificacionAsync(string numeroIdentificacion, int? excludeId = null)
        => await _terceroRepository.ExistsByIdentificacionAsync(numeroIdentificacion, excludeId);

    public async Task<ActionResponse<Tercero>> GetByIdentificacionAsync(string numeroIdentificacion)
        => await _terceroRepository.GetByIdentificacionAsync(numeroIdentificacion);

    public async Task<ActionResponse<IEnumerable<Tercero>>> GetTercerosActivosAsync()
        => await _terceroRepository.GetTercerosActivosAsync();

    public async Task<ActionResponse<IEnumerable<Tercero>>> GetTercerosClienteAsync()
        => await _terceroRepository.GetTercerosClienteAsync();

    public async Task<ActionResponse<IEnumerable<Tercero>>> GetTercerosProveedorAsync()
        => await _terceroRepository.GetTercerosProveedorAsync();

    public async Task<ActionResponse<Tercero>> UpdateByIdentificacionAsync(string numeroIdentificacion, Tercero updatedTercero)
        => await _terceroRepository.UpdateByIdentificacionAsync(numeroIdentificacion, updatedTercero);
}
