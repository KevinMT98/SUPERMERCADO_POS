using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

    public class TiposIdentificacionRepository : GenericRepository<TiposIdentificacion>, ITiposIdentificacionRepository
    {
        private readonly DataContext _context;
        public TiposIdentificacionRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByDescripcionAsync(string descripcion, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _context.TiposIdentificacions
                    .AnyAsync(t => t.descripcion == descripcion && t.ID != excludeId.Value);
            }
            return await _context.TiposIdentificacions
                .AnyAsync(t => t.descripcion == descripcion);
        }

    public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.TiposIdentificacions
                .AnyAsync(t => t.tipoDocumentoID == codigo && t.ID != excludeId.Value);
        }
        return await _context.TiposIdentificacions
            .AnyAsync(t => t.tipoDocumentoID == codigo);
    }

    public async Task<ActionResponse<TiposIdentificacion>> GetByDescripcionAsync(string descripcion)
        {
            try
            {
                var tipoIdentificacion = await _context.TiposIdentificacions
                    .FirstOrDefaultAsync(t => t.descripcion == descripcion);
                if (tipoIdentificacion == null)
                {
                    return new ActionResponse<TiposIdentificacion>
                    {
                        WasSuccess = false,
                        Message = "No se encontró el tipo de identificación con la descripción especificada."
                    };
                }
                return new ActionResponse<TiposIdentificacion>
                {
                    WasSuccess = true,
                    Result = tipoIdentificacion
                };
            }
            catch (Exception exception)
            {
                return new ActionResponse<TiposIdentificacion>
                {
                    WasSuccess = false,
                    Message = exception.Message
                };
            }
        }

    public override async Task<ActionResponse<TiposIdentificacion>> DeleteAsync(int id)
    {
        // Verificar si la categoría existe
        var tiposIdentificacionExist = await GetAsync(id);
        if (!tiposIdentificacionExist.WasSuccess)
        {
            return new ActionResponse<TiposIdentificacion>
            {
                WasSuccess = false,
                Message = "El tipo de documento no existe."
            };
        }

        // Verificar si tiene productos asociados
        var tieneTerceroAsociado = await _context.Terceros
            .AnyAsync(te => te.FK_codigo_ident == id);

        if (tieneTerceroAsociado)
        {
            return new ActionResponse<TiposIdentificacion>
            {
                WasSuccess = false,
                Message = "No se puede eliminar el tipo de documento porque tiene productos asociados."
            };
        }

        // Si no tiene productos, proceder con la eliminación
        return await base.DeleteAsync(id);
    }
}