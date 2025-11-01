using Supermercado.Backend.Repositories.Implementations;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

public class ConsecutivoUnitOfWork : GenericUnitOfWork<Consecutivo>, IConsecutivoUnitOfWork
{
    private readonly IConsecutivoRepository _consecutivoRepository;
    public ConsecutivoUnitOfWork(IConsecutivoRepository consecutivoRepository) : base(consecutivoRepository)
    {
        _consecutivoRepository = consecutivoRepository;
    }

    public override async Task<ActionResponse<Consecutivo>> AddAsync(Consecutivo entity)
    {
        if (entity.consecutivo_ini > entity.consecutivo_fin)
        {
            return new ActionResponse<Consecutivo>
            {
                WasSuccess = false,
                Message = "El consecutivo inicial no puede ser mayor que el consecutivo final."
            };
        }

        return await _consecutivoRepository.AddAsync(entity);
    }

}
