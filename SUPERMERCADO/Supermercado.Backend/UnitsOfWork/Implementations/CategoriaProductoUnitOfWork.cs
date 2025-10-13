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

    public async Task<bool> TieneProductosAsociadosAsync(int categoriaId)
        => await _categoriaProductoRepository.TieneProductosAsociadosAsync(categoriaId);

    public async Task<ActionResponse<IEnumerable<Categoria_Producto>>> GetCategoriasActivasAsync()
        => await _categoriaProductoRepository.GetCategoriasActivasAsync();

    /// <summary>
    /// Elimina una categoría validando que no tenga productos asociados
    /// </summary>
    /// <param name="id">ID de la categoría a eliminar</param>
    /// <returns>ActionResponse indicando el resultado de la operación</returns>
    public override async Task<ActionResponse<Categoria_Producto>> DeleteAsync(int id)
    {
        // Verificar si la categoría existe
        var categoriaExistente = await _categoriaProductoRepository.GetAsync(id);
        if (!categoriaExistente.WasSuccess)
        {
            return new ActionResponse<Categoria_Producto>
            {
                WasSuccess = false,
                Message = "La categoría no existe."
            };
        }

        // Verificar si tiene productos asociados
        var tieneProductos = await _categoriaProductoRepository.TieneProductosAsociadosAsync(id);
        if (tieneProductos)
        {
            return new ActionResponse<Categoria_Producto>
            {
                WasSuccess = false,
                Message = "No se puede eliminar la categoría porque tiene productos asociados."
            };
        }

        // Si no tiene productos, proceder con la eliminación
        return await base.DeleteAsync(id);
    }
}
