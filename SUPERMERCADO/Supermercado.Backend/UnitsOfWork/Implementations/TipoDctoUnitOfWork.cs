using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.UnitsOfWork.Implementations;

public class TipoDctoUnitOfWork : GenericUnitOfWork<TipoDcto>, ITipoDctoUnitOfWork
{
    private readonly ITipoDctoRepository _tipoDctoRepository;

    public TipoDctoUnitOfWork(ITipoDctoRepository tipoDctoRepository) : base(tipoDctoRepository)
    {
        _tipoDctoRepository = tipoDctoRepository;
    }
}
