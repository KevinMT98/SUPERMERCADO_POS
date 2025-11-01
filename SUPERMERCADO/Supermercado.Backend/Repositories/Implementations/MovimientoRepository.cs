using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio para manejar operaciones de datos de Movimientos
/// </summary>
public class MovimientoRepository : GenericRepository<Movimiento>, IMovimientoRepository
{
    private readonly DataContext _context;

    public MovimientoRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los movimientos con sus relaciones
    /// </summary>
    public override async Task<ActionResponse<IEnumerable<Movimiento>>> GetAsync()
    {
        var movimientos = await _context.Movimientos
            .Include(m => m.TipoDcto)
            .Include(m => m.Consecutivo)
            .Include(m => m.Usuario)
            .Include(m => m.Tercero)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Movimiento>>
        {
            WasSuccess = true,
            Result = movimientos
        };
    }

    /// <summary>
    /// Obtiene un movimiento por ID con sus relaciones
    /// </summary>
    public override async Task<ActionResponse<Movimiento>> GetAsync(int id)
    {
        var movimiento = await _context.Movimientos
            .Include(m => m.TipoDcto)
            .Include(m => m.Consecutivo)
            .Include(m => m.Usuario)
            .Include(m => m.Tercero)
            .FirstOrDefaultAsync(m => m.movimiento_id == id);

        if (movimiento == null)
        {
            return new ActionResponse<Movimiento>
            {
                WasSuccess = false,
                Message = "Movimiento no encontrado"
            };
        }

        return new ActionResponse<Movimiento>
        {
            WasSuccess = true,
            Result = movimiento
        };
    }
}
