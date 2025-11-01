using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio para manejar operaciones de datos de Facturas
/// </summary>
public class FacturaRepository : GenericRepository<Factura>, IFacturaRepository
{
    private readonly DataContext _context;

    public FacturaRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todas las facturas con sus relaciones
    /// </summary>
    public override async Task<ActionResponse<IEnumerable<Factura>>> GetAsync()
    {
        var facturas = await _context.Facturas
            .Include(f => f.Movimiento)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Factura>>
        {
            WasSuccess = true,
            Result = facturas
        };
    }

    /// <summary>
    /// Obtiene una factura por ID con sus relaciones
    /// </summary>
    public override async Task<ActionResponse<Factura>> GetAsync(int id)
    {
        var factura = await _context.Facturas
            .Include(f => f.Movimiento)
            .FirstOrDefaultAsync(f => f.factura_id == id);

        if (factura == null)
        {
            return new ActionResponse<Factura>
            {
                WasSuccess = false,
                Message = "Factura no encontrada"
            };
        }

        return new ActionResponse<Factura>
        {
            WasSuccess = true,
            Result = factura
        };
    }
}
