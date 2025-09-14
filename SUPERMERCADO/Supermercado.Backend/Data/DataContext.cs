using Microsoft.EntityFrameworkCore;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Categoria_Producto> Categoria_Productos { get; set; }
    public DbSet<Rol> Rols { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Rol>().HasIndex(x => x.nombre).IsUnique();
        modelBuilder.Entity<Categoria_Producto>().HasIndex(x => x.descripcion).IsUnique();
    }
}
