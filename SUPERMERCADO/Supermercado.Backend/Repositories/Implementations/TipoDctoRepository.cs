using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;


namespace Supermercado.Backend.Repositories.Implementations;

public class TipoDctoRepository : GenericRepository<TipoDcto>, ITipoDctoRepository
{
    private readonly DataContext _context;
    public TipoDctoRepository(DataContext context) : base(context)
    {
        _context = context;
    }
}
