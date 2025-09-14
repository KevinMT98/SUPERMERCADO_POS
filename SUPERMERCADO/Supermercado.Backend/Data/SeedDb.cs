using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Data;
//clase para creacion de base de datos y datos iniciales
public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckRolsAsync();
        await CheckCategoria_ProductosAsync();
    }

    private async Task CheckCategoria_ProductosAsync()
    {
        if (!_context.Categoria_Productos.Any())
        {
            _context.Categoria_Productos.Add(new Categoria_Producto { descripcion = "Bebidas" });
            _context.Categoria_Productos.Add(new Categoria_Producto { descripcion = "Lacteos" });
            _context.Categoria_Productos.Add(new Categoria_Producto { descripcion = "Aseo" });
            _context.Categoria_Productos.Add(new Categoria_Producto { descripcion = "Comida" });
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckRolsAsync()
    {
        if (!_context.Rols.Any())
        {
            _context.Rols.Add(new Rol { nombre = "Admin" });
            _context.Rols.Add(new Rol { nombre = "User" });
            await _context.SaveChangesAsync();
        }
    }
}
