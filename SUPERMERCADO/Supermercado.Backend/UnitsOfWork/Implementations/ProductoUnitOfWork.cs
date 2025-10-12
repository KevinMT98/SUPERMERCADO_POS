using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unidad de trabajo específica para Producto
/// Coordina operaciones del repositorio de Producto y expone métodos personalizados
/// </summary>
public class ProductoUnitOfWork : GenericUnitOfWork<Producto>, IProductoUnitOfWork
{
    private readonly IProductoRepository _productoRepository;

    public ProductoUnitOfWork(IProductoRepository productoRepository) : base(productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<bool> ExistsByCodigoProductoAsync(string codigoProducto, int? excludeId = null)
        => await _productoRepository.ExistsByCodigoProductoAsync(codigoProducto, excludeId);

    public async Task<bool> ExistsByCodigoBarrasAsync(string codigoBarras, int? excludeId = null)
        => await _productoRepository.ExistsByCodigoBarrasAsync(codigoBarras, excludeId);

    public async Task<ActionResponse<IEnumerable<Producto>>> GetProductosConStockBajoAsync()
        => await _productoRepository.GetProductosConStockBajoAsync();

    public async Task<ActionResponse<IEnumerable<Producto>>> GetProductosByCategoriaAsync(int categoriaId)
        => await _productoRepository.GetProductosByCategoriaAsync(categoriaId);
}
