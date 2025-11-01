using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio para manejar operaciones de datos de Métodos de Pago
/// </summary>
public class MetodosPagoRepository : GenericRepository<Metodos_Pago>, IMetodosPagoRepository
{
    private readonly DataContext _context;

    public MetodosPagoRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene todos los métodos de pago
    /// </summary>
    public override async Task<ActionResponse<IEnumerable<Metodos_Pago>>> GetAsync()
    {
        var metodosPago = await _context.MetodosPago
            .ToListAsync();

        return new ActionResponse<IEnumerable<Metodos_Pago>>
        {
            WasSuccess = true,
            Result = metodosPago
        };
    }

    /// <summary>
    /// Obtiene un método de pago por ID
    /// </summary>
    public override async Task<ActionResponse<Metodos_Pago>> GetAsync(int id)
    {
        var metodoPago = await _context.MetodosPago
            .FirstOrDefaultAsync(m => m.id_metodo_pago == id);

        if (metodoPago == null)
        {
            return new ActionResponse<Metodos_Pago>
            {
                WasSuccess = false,
                Message = "Método de pago no encontrado"
            };
        }

        return new ActionResponse<Metodos_Pago>
        {
            WasSuccess = true,
            Result = metodoPago
        };
    }

    public async Task<ActionResponse<Metodos_Pago>> GetByCodigoAsync(string codigo)
    {
        var metodoPago = await _context.MetodosPago
            .FirstOrDefaultAsync(m => m.codigo_metpag == codigo);
        if (metodoPago == null)
        {
            return new ActionResponse<Metodos_Pago>
            {
                WasSuccess = false,
                Message = "Método de pago no encontrado"
            };
        }
        return new ActionResponse<Metodos_Pago>
        {
            WasSuccess = true,
            Result = metodoPago
        };
    }
}
