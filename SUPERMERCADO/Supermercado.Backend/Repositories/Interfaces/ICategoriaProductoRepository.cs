using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Categoria_Producto con validaciones personalizadas
/// </summary>
public interface ICategoriaProductoRepository : IGenericRepository<Categoria_Producto>
{
    /// <summary>
    /// Verifica si existe una categoría con la descripción especificada
    /// </summary>
    /// <param name="descripcion">Descripción de la categoría a verificar</param>
    /// <param name="excludeId">ID de la categoría a excluir de la búsqueda (útil para actualizaciones)</param>
    /// <returns>True si existe, False si no existe</returns>
    Task<bool> ExistsByDescripcionAsync(string descripcion, int? excludeId = null);

    /// <summary>
    /// Obtiene una categoría por su descripción
    /// </summary>
    /// <param name="descripcion">Descripción de la categoría a buscar</param>
    /// <returns>Categoría encontrada</returns>
    Task<ActionResponse<Categoria_Producto>> GetByDescripcionAsync(string descripcion);

    /// <summary>
    /// Obtiene categorías con productos asociados
    /// </summary>
    /// <returns>Lista de categorías que tienen productos</returns>
    Task<ActionResponse<IEnumerable<Categoria_Producto>>> GetCategoriasConProductosAsync();
}
