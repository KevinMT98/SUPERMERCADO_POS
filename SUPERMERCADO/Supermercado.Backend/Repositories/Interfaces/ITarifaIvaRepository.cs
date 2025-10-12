using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Interfaces;

/// <summary>
/// Interfaz específica para el repositorio de Tarifa_IVA con validaciones personalizadas
/// </summary>
public interface ITarifaIvaRepository : IGenericRepository<Tarifa_IVA>
{
    /// <summary>
    /// Verifica si existe una tarifa IVA con el código especificado
    /// </summary>
    /// <param name="codigoIva">Código IVA a verificar</param>
    /// <param name="excludeId">ID de la tarifa a excluir de la búsqueda (útil para actualizaciones)</param>
    /// <returns>True si existe, False si no existe</returns>
    Task<bool> ExistsByCodigoIvaAsync(string codigoIva, int? excludeId = null);

    /// <summary>
    /// Obtiene todas las tarifas IVA activas
    /// </summary>
    /// <returns>Lista de tarifas IVA activas</returns>
    Task<ActionResponse<IEnumerable<Tarifa_IVA>>> GetTarifasActivasAsync();

    /// <summary>
    /// Obtiene una tarifa IVA por su código
    /// </summary>
    /// <param name="codigoIva">Código IVA a buscar</param>
    /// <returns>Tarifa IVA encontrada</returns>
    Task<ActionResponse<Tarifa_IVA>> GetByCodigoIvaAsync(string codigoIva);
}
