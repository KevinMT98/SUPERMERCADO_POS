using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio para manejar operaciones de datos de Detalle de Factura
/// </summary>
public class DetalleFacturaRepository : GenericRepository<Detalle_Factura>, IDetalleFacturaRepository
{
    private readonly DataContext _context;

    public DetalleFacturaRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los detalles de factura con sus relaciones
    /// </summary>
    public override async Task<ActionResponse<IEnumerable<Detalle_Factura>>> GetAsync()
    {
        var detalles = await _context.DetallesFactura
            .Include(d => d.Factura)
            .Include(d => d.Producto)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Detalle_Factura>>
        {
            WasSuccess = true,
            Result = detalles
        };
    }

    /// <summary>
    /// Obtiene un detalle de factura por ID con sus relaciones
    /// </summary>
    public override async Task<ActionResponse<Detalle_Factura>> GetAsync(int id)
    {
        var detalle = await _context.DetallesFactura
            .Include(d => d.Factura)
            .Include(d => d.Producto)
            .FirstOrDefaultAsync(d => d.detalle_id == id);

        if (detalle == null)
        {
            return new ActionResponse<Detalle_Factura>
            {
                WasSuccess = false,
                Message = "Detalle de factura no encontrado"
            };
        }

        return new ActionResponse<Detalle_Factura>
        {
            WasSuccess = true,
            Result = detalle
        };
    }
}
