using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Interfaces;


public interface IMetodosPagoRepository : IGenericRepository<Metodos_Pago>
{
    public Task<ActionResponse<Metodos_Pago>> GetByCodigoAsync(string codigo);

}
