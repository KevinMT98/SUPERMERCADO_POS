using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;

public interface ITiposIdentificacionUnitOfWork : IGenericUnitOfWork<TiposIdentificacion>
{
    Task<bool> ExistsByDescripcionAsync(string descripcion, int? excludeId = null);
    Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null);
    Task<ActionResponse<TiposIdentificacion>> GetByDescripcionAsync(string descripcion);
    

}
