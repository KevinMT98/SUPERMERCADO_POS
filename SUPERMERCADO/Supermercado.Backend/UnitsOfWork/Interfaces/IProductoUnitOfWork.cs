using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Interfaces;

/// <summary>
/// Interfaz específica para la unidad de trabajo de Producto
/// Expone operaciones CRUD genéricas y métodos personalizados de validación
/// </summary>
public interface IProductoUnitOfWork : IGenericUnitOfWork<Producto>
{

    Task<bool> ExistsByCodigoProductoAsync(string codigoProducto, int? excludeId = null);


    Task<bool> ExistsByCodigoBarrasAsync(string codigoBarras, int? excludeId = null);


    Task<ActionResponse<IEnumerable<Producto>>> GetProductosConStockBajoAsync();


    Task<ActionResponse<IEnumerable<Producto>>> GetProductosByCategoriaAsync(int categoriaId);
}
