

using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Repositories.Implementations;

public class ConsecutivoRepository : GenericRepository<Consecutivo>, IConsecutivoRepository
{
    private readonly DataContext _context;
    public ConsecutivoRepository(DataContext context) : base(context)
    {
        _context = context;
    }


}
