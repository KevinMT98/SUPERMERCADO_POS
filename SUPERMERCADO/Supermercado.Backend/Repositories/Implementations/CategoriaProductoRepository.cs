using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio específico para Categoria_Producto con validaciones y lógica personalizada
/// </summary>
public class CategoriaProductoRepository : GenericRepository<Categoria_Producto>, ICategoriaProductoRepository
{
    private readonly DataContext _context;

    public CategoriaProductoRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByDescripcionAsync(string descripcion, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Categoria_Productos
                .AnyAsync(c => c.descripcion == descripcion && c.categoriaId != excludeId.Value);
        }
        
        return await _context.Categoria_Productos
            .AnyAsync(c => c.descripcion == descripcion);
    }

    public async Task<ActionResponse<Categoria_Producto>> GetByDescripcionAsync(string descripcion)
    {
        try
        {
            var categoria = await _context.Categoria_Productos
                .FirstOrDefaultAsync(c => c.descripcion == descripcion);

            if (categoria == null)
            {
                return new ActionResponse<Categoria_Producto>
                {
                    WasSuccess = false,
                    Message = "No se encontró la categoría con la descripción especificada."
                };
            }

            return new ActionResponse<Categoria_Producto>
            {
                WasSuccess = true,
                Result = categoria
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<Categoria_Producto>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<Categoria_Producto>>> GetCategoriasConProductosAsync()
    {
        try
        {
            var categorias = await _context.Categoria_Productos
                .Where(c => _context.Productos.Any(p => p.FK_categoria_id == c.categoriaId))
                .OrderBy(c => c.descripcion)
                .ToListAsync();

            return new ActionResponse<IEnumerable<Categoria_Producto>>
            {
                WasSuccess = true,
                Result = categorias
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<IEnumerable<Categoria_Producto>>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<bool> TieneProductosAsociadosAsync(int categoriaId)
    {
        return await _context.Productos
            .AnyAsync(p => p.FK_categoria_id == categoriaId);
    }

    public async Task<ActionResponse<IEnumerable<Categoria_Producto>>> GetCategoriasActivasAsync()
    {
        try
        {
            var categorias = await _context.Categoria_Productos
                .Where(c => c.activo)
                .OrderBy(c => c.descripcion)
                .ToListAsync();

            return new ActionResponse<IEnumerable<Categoria_Producto>>
            {
                WasSuccess = true,
                Result = categorias
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<IEnumerable<Categoria_Producto>>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}
