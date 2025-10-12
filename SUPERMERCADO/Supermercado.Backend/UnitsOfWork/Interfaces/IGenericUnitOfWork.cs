using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;

public interface IGenericUnitOfWork<T> where T : class
{
    Task<ActionResponse<T>> GetAsync(int id);
    Task<ActionResponse<IEnumerable<T>>> GetAsync();
    Task<ActionResponse<T>> AddAsync(T entity);
    Task<ActionResponse<T>> DeleteAsync(int id);
    Task<ActionResponse<T>> UpdateAsync(T entity);
    Task<bool> ExistsByCodigoProductoAsync(string codigoProducto);
    Task<bool> ExistsByCodigoBarrasAsync(string codigoBarras);
}

