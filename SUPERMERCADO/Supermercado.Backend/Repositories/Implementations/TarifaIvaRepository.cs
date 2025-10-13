using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio específico para Tarifa_IVA con validaciones y lógica personalizada
/// </summary>
public class TarifaIvaRepository : GenericRepository<Tarifa_IVA>, ITarifaIvaRepository
{
    private readonly DataContext _context;

    public TarifaIvaRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCodigoIvaAsync(string codigoIva, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Tarifa_IVAs
                .AnyAsync(t => t.codigo_Iva == codigoIva && t.tarifa_iva_id != excludeId.Value);
        }
        
        return await _context.Tarifa_IVAs
            .AnyAsync(t => t.codigo_Iva == codigoIva);
    }

    public async Task<ActionResponse<IEnumerable<Tarifa_IVA>>> GetTarifasActivasAsync()
    {
        try
        {
            var tarifas = await _context.Tarifa_IVAs
                .Where(t => t.estado)
                .OrderBy(t => t.porcentaje)
                .ToListAsync();

            return new ActionResponse<IEnumerable<Tarifa_IVA>>
            {
                WasSuccess = true,
                Result = tarifas
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<IEnumerable<Tarifa_IVA>>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<Tarifa_IVA>> GetByCodigoIvaAsync(string codigoIva)
    {
        try
        {
            var tarifa = await _context.Tarifa_IVAs
                .FirstOrDefaultAsync(t => t.codigo_Iva == codigoIva);

            if (tarifa == null)
            {
                return new ActionResponse<Tarifa_IVA>
                {
                    WasSuccess = false,
                    Message = "No se encontró la tarifa IVA con el código especificado."
                };
            }

            return new ActionResponse<Tarifa_IVA>
            {
                WasSuccess = true,
                Result = tarifa
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Tarifa_IVA>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<bool> TieneProductosAsociadosAsync(int tarifaIvaId)
    {
        return await _context.Productos
            .AnyAsync(p => p.FK_codigo_iva == tarifaIvaId);
    }
}
