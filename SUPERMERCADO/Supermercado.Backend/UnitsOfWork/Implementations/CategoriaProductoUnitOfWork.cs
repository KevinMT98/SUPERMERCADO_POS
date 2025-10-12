using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unidad de trabajo específica para Categoria_Producto
/// Coordina operaciones del repositorio de Categoria_Producto y expone métodos personalizados
/// </summary>
public class CategoriaProductoUnitOfWork : GenericUnitOfWork<Categoria_Producto>, ICategoriaProductoUnitOfWork
{
    private readonly ICategoriaProductoRepository _categoriaProductoRepository;

    public CategoriaProductoUnitOfWork(ICategoriaProductoRepository categoriaProductoRepository) 
        : base(categoriaProductoRepository)
    {
        _categoriaProductoRepository = categoriaProductoRepository;
    }

    public async Task<bool> ExistsByDescripcionAsync(string descripcion, int? excludeId = null)
        => await _categoriaProductoRepository.ExistsByDescripcionAsync(descripcion, excludeId);

    public async Task<ActionResponse<Categoria_Producto>> GetByDescripcionAsync(string descripcion)
        => await _categoriaProductoRepository.GetByDescripcionAsync(descripcion);

    public async Task<ActionResponse<IEnumerable<Categoria_Producto>>> GetCategoriasConProductosAsync()
        => await _categoriaProductoRepository.GetCategoriasConProductosAsync();
}
