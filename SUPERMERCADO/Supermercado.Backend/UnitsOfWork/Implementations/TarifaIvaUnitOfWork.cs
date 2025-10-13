using Supermercado.Backend.Repositories.Implementations;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

/// <summary>
/// Unidad de trabajo específica para Tarifa_IVA
/// Coordina operaciones del repositorio de Tarifa_IVA y expone métodos personalizados
/// </summary>
public class TarifaIvaUnitOfWork : GenericUnitOfWork<Tarifa_IVA>, ITarifaIvaUnitOfWork
{
    private readonly ITarifaIvaRepository _tarifaIvaRepository;

    public TarifaIvaUnitOfWork(ITarifaIvaRepository tarifaIvaRepository) : base(tarifaIvaRepository)
    {
        _tarifaIvaRepository = tarifaIvaRepository;
    }

    public async Task<bool> ExistsByCodigoIvaAsync(string codigoIva, int? excludeId = null)
        => await _tarifaIvaRepository.ExistsByCodigoIvaAsync(codigoIva, excludeId);

    public async Task<ActionResponse<IEnumerable<Tarifa_IVA>>> GetTarifasActivasAsync()
        => await _tarifaIvaRepository.GetTarifasActivasAsync();

    public async Task<ActionResponse<Tarifa_IVA>> GetByCodigoIvaAsync(string codigoIva)
        => await _tarifaIvaRepository.GetByCodigoIvaAsync(codigoIva);

    public async Task<bool> TieneProductosAsociadosAsync(int tarifaIvaId)
        => await _tarifaIvaRepository.TieneProductosAsociadosAsync(tarifaIvaId);

    /// <summary>
    /// Elimina una tarifa IVA validando que no tenga productos asociados
    /// </summary>
    /// <param name="id">ID de la tarifa IVA a eliminar</param>
    /// <returns>ActionResponse indicando el resultado de la operación</returns>
    public override async Task<ActionResponse<Tarifa_IVA>> DeleteAsync(int id)
    {
        // Verificar si la tarifa IVA existe
        var tarifaExistente = await _tarifaIvaRepository.GetAsync(id);
        if (!tarifaExistente.WasSuccess)
        {
            return new ActionResponse<Tarifa_IVA>
            {
                WasSuccess = false,
                Message = "La tarifa IVA no existe."
            };
        }

        // Verificar si tiene productos asociados
        var tieneProductos = await _tarifaIvaRepository.TieneProductosAsociadosAsync(id);
        if (tieneProductos)
        {
            return new ActionResponse<Tarifa_IVA>
            {
                WasSuccess = false,
                Message = "No se puede eliminar la tarifa IVA porque tiene productos asociados."
            };
        }

        // Si no tiene productos, proceder con la eliminación
        return await base.DeleteAsync(id);
    }


}
