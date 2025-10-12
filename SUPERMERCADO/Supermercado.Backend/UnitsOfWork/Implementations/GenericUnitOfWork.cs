using Supermercado.Backend.Repositories.Implementations;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

public class GenericUnitOfWork<T> : IGenericUnitOfWork<T> where T : class
{
    private readonly IGenericRepository<T> _repository;

    public GenericUnitOfWork(IGenericRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual async Task<ActionResponse<T>> AddAsync(T entity) => await _repository.AddAsync(entity);

    public virtual async Task<ActionResponse<T>> DeleteAsync(int id) => await _repository.DeleteAsync(id);

    public virtual async Task<ActionResponse<T>> GetAsync(int id) => await _repository.GetAsync(id);

    public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync() => await _repository.GetAsync();

    public virtual async Task<ActionResponse<T>> UpdateAsync(T entity) => await _repository.UpdateAsync(entity);


    public async Task<bool> ExistsByCodigoProductoAsync(string codigoProducto)
    {
        // Solo tiene sentido para Producto, puedes lanzar excepción para otros tipos
        if (_repository is GenericRepository<Producto> productoRepo)
            return await productoRepo.ExistsByCodigoProductoAsync(codigoProducto);
        throw new NotSupportedException("Este método solo es válido para Producto.");
    }

    public async Task<bool> ExistsByCodigoBarrasAsync(string codigoBarras)
    {
        if (_repository is GenericRepository<Producto> productoRepo)
            return await productoRepo.ExistsByCodigoBarrasAsync(codigoBarras);
        throw new NotSupportedException("Este método solo es válido para Producto.");
    }
}
