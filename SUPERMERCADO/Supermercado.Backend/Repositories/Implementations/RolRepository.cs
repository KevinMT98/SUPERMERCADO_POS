using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio específico para Rol con validaciones y lógica personalizada
/// </summary>
public class RolRepository : GenericRepository<Rol>, IRolRepository
{
    private readonly DataContext _context;

    public RolRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByNombreAsync(string nombre, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Rols
                .AnyAsync(r => r.nombre == nombre && r.rol_id != excludeId.Value);
        }
        
        return await _context.Rols
            .AnyAsync(r => r.nombre == nombre);
    }

    public async Task<ActionResponse<Rol>> GetByNombreAsync(string nombre)
    {
        try
        {
            var rol = await _context.Rols
                .FirstOrDefaultAsync(r => r.nombre == nombre);

            if (rol == null)
            {
                return new ActionResponse<Rol>
                {
                    WasSuccess = false,
                    Message = "No se encontró el rol con el nombre especificado."
                };
            }

            return new ActionResponse<Rol>
            {
                WasSuccess = true,
                Result = rol
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Rol>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}
