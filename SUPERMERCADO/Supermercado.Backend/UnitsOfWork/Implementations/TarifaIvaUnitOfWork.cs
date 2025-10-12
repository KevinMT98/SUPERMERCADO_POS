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
}
