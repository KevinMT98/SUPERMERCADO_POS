using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio para manejar operaciones de datos de Pago de Factura
/// </summary>
public class PagoFacturaRepository : GenericRepository<Pago_Factura>, IPagoFacturaRepository
{
    private readonly DataContext _context;

    public PagoFacturaRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los pagos de factura con sus relaciones
    /// </summary>
    public override async Task<ActionResponse<IEnumerable<Pago_Factura>>> GetAsync()
    {
        var pagos = await _context.PagosFactura
            .Include(p => p.Factura)
            .Include(p => p.MetodoPago)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Pago_Factura>>
        {
            WasSuccess = true,
            Result = pagos
        };
    }

    /// <summary>
    /// Obtiene un pago de factura por ID con sus relaciones
    /// </summary>
    public override async Task<ActionResponse<Pago_Factura>> GetAsync(int id)
    {
        var pago = await _context.PagosFactura
            .Include(p => p.Factura)
            .Include(p => p.MetodoPago)
            .FirstOrDefaultAsync(p => p.pago_id == id);

        if (pago == null)
        {
            return new ActionResponse<Pago_Factura>
            {
                WasSuccess = false,
                Message = "Pago de factura no encontrado"
            };
        }

        return new ActionResponse<Pago_Factura>
        {
            WasSuccess = true,
            Result = pago
        };
    }
}
