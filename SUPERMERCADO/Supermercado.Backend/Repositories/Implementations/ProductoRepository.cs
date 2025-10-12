using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Shared.Entities;
using Supermercado.Shared.Responses;

namespace Supermercado.Backend.Repositories.Implementations;

/// <summary>
/// Repositorio específico para Producto con validaciones y lógica personalizada
/// </summary>
public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
{
    private readonly DataContext _context;

    public ProductoRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> ExistsByCodigoProductoAsync(string codigoProducto, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Productos
                .AnyAsync(p => p.codigo_producto == codigoProducto && p.producto_id != excludeId.Value);
        }
        
        return await _context.Productos
            .AnyAsync(p => p.codigo_producto == codigoProducto);
    }

    public async Task<bool> ExistsByCodigoBarrasAsync(string codigoBarras, int? excludeId = null)
    {
        if (excludeId.HasValue)
        {
            return await _context.Productos
                .AnyAsync(p => p.codigo_barras == codigoBarras && p.producto_id != excludeId.Value);
        }
        
        return await _context.Productos
            .AnyAsync(p => p.codigo_barras == codigoBarras);
    }

    public async Task<ActionResponse<IEnumerable<Producto>>> GetProductosConStockBajoAsync()
    {
        try
        {
            var productos = await _context.Productos
                .Where(p => p.stock_actual < p.stock_minimo && p.activo)
                .Include(p => p.Categoria)
                .Include(p => p.TarifaIVA)
                .ToListAsync();

            return new ActionResponse<IEnumerable<Producto>>
            {
                WasSuccess = true,
                Result = productos
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<IEnumerable<Producto>>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }

    public async Task<ActionResponse<IEnumerable<Producto>>> GetProductosByCategoriaAsync(int categoriaId)
    {
        try
        {
            var productos = await _context.Productos
                .Where(p => p.FK_categoria_id == categoriaId)
                .Include(p => p.Categoria)
                .Include(p => p.TarifaIVA)
                .ToListAsync();

            return new ActionResponse<IEnumerable<Producto>>
            {
                WasSuccess = true,
                Result = productos
            };
        }
        catch (Exception exception)
        {
            return new ActionResponse<IEnumerable<Producto>>
            {
                WasSuccess = false,
                Message = exception.Message
            };
        }
    }
}
