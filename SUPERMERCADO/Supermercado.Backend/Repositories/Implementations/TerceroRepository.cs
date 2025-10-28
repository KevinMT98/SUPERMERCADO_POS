using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

public class TerceroRepository : GenericRepository<Tercero>, ITerceroRepository
{
    private readonly DataContext _context;

    public TerceroRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByIdentificacionAsync(string numeroIdentificacion, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Terceros
                .AnyAsync(t => t.numero_identificacion == numeroIdentificacion && t.tercero_id != excludeId.Value);
        }

        return await _context.Terceros
            .AnyAsync(t => t.numero_identificacion == numeroIdentificacion);
    }

    public async Task<ActionResponse<Tercero>> GetByIdentificacionAsync(string numeroIdentificacion)
    {
        try
        {
            var tercero = await _context.Terceros
                .FirstOrDefaultAsync(t => t.numero_identificacion == numeroIdentificacion);
            if (tercero == null)
            {
                return new ActionResponse<Tercero>
                {
                    WasSuccess = false,
                    Message = "No se encontró el tercero con el número de identificación especificado."
                };
            }
            return new ActionResponse<Tercero>
            {
                WasSuccess = true,
                Result = tercero
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Tercero>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<Tercero>>> GetTercerosActivosAsync()
    {
        try
        {
            var terceros = await _context.Terceros
                .Where(t => t.activo == true)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Tercero>>
            {
                WasSuccess = true,
                Result = terceros
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<IEnumerable<Tercero>>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<Tercero>>> GetTercerosClienteAsync()
    {
        try
        {
            var terceros = await _context.Terceros
                .Where(t => t.es_cliente == true)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Tercero>>
            {
                WasSuccess = true,
                Result = terceros
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<IEnumerable<Tercero>>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<Tercero>>> GetTercerosProveedorAsync()
    {
        try
        {
            var terceros = await _context.Terceros
                .Where(t => t.es_proveedor == true)
                .ToListAsync();
            return new ActionResponse<IEnumerable<Tercero>>
            {
                WasSuccess = true,
                Result = terceros
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<IEnumerable<Tercero>>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<Tercero>> UpdateByIdentificacionAsync(string numeroIdentificacion, Tercero updatedTercero)
    {
        try
        {
            var existingTercero = await _context.Terceros
                .FirstOrDefaultAsync(t => t.numero_identificacion == numeroIdentificacion);
            
            if (existingTercero == null)
            {
                return new ActionResponse<Tercero>
                {
                    WasSuccess = false,
                    Message = "No se encontró el tercero con el número de identificación especificado."
                };
            }

            // Actualizar los campos del tercero existente
            existingTercero.FK_codigo_ident = updatedTercero.FK_codigo_ident;
            existingTercero.nombre = updatedTercero.nombre;
            existingTercero.nombre2 = updatedTercero.nombre2;
            existingTercero.apellido1 = updatedTercero.apellido1;
            existingTercero.apellido2 = updatedTercero.apellido2;
            existingTercero.email = updatedTercero.email;
            existingTercero.direccion = updatedTercero.direccion;
            existingTercero.telefono = updatedTercero.telefono;
            existingTercero.activo = updatedTercero.activo;
            existingTercero.es_proveedor = updatedTercero.es_proveedor;
            existingTercero.es_cliente = updatedTercero.es_cliente;

            await _context.SaveChangesAsync();
            
            return new ActionResponse<Tercero>
            {
                WasSuccess = true,
                Result = existingTercero
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Tercero>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}