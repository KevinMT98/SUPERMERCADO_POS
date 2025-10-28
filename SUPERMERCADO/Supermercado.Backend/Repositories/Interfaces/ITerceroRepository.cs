using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Interfaces;

public interface ITerceroRepository : IGenericRepository<Tercero>
{
    Task<bool> ExistsByIdentificacionAsync(string numeroIdentificacion, int? excludeId = null);
    Task<ActionResponse<Tercero>> GetByIdentificacionAsync(string numeroIdentificacion);
    Task<ActionResponse<IEnumerable<Tercero>>> GetTercerosActivosAsync();
    Task<ActionResponse<IEnumerable<Tercero>>> GetTercerosClienteAsync();
    Task<ActionResponse<IEnumerable<Tercero>>> GetTercerosProveedorAsync();
    Task<ActionResponse<Tercero>> UpdateByIdentificacionAsync(string numeroIdentificacion, Tercero updatedTercero);
}
