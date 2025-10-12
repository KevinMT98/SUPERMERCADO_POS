using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;

/// <summary>
/// Interfaz específica para la unidad de trabajo de Producto
/// Expone operaciones CRUD genéricas y métodos personalizados de validación
/// </summary>
public interface IProductoUnitOfWork : IGenericUnitOfWork<Producto>
{
    /// <summary>
    /// Verifica si existe un producto con el código de producto especificado
    /// </summary>
    /// <param name="codigoProducto">Código del producto a verificar</param>
    /// <param name="excludeId">ID del producto a excluir de la búsqueda (útil para actualizaciones)</param>
    /// <returns>True si existe, False si no existe</returns>
    Task<bool> ExistsByCodigoProductoAsync(string codigoProducto, int? excludeId = null);

    /// <summary>
    /// Verifica si existe un producto con el código de barras especificado
    /// </summary>
    /// <param name="codigoBarras">Código de barras a verificar</param>
    /// <param name="excludeId">ID del producto a excluir de la búsqueda (útil para actualizaciones)</param>
    /// <returns>True si existe, False si no existe</returns>
    Task<bool> ExistsByCodigoBarrasAsync(string codigoBarras, int? excludeId = null);

    /// <summary>
    /// Obtiene productos con stock bajo (menor al stock mínimo)
    /// </summary>
    /// <returns>Lista de productos con stock bajo</returns>
    Task<ActionResponse<IEnumerable<Producto>>> GetProductosConStockBajoAsync();

    /// <summary>
    /// Obtiene productos por categoría
    /// </summary>
    /// <param name="categoriaId">ID de la categoría</param>
    /// <returns>Lista de productos de la categoría</returns>
    Task<ActionResponse<IEnumerable<Producto>>> GetProductosByCategoriaAsync(int categoriaId);
}
