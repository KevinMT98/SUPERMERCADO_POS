using Microsoft.EntityFrameworkCore;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Categoria_Producto> Categoria_Productos { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Rol> Rols { get; set; }
    public DbSet<Tarifa_IVA> Tarifa_IVAs { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Rol>().HasIndex(x => x.nombre).IsUnique();
        modelBuilder.Entity<Categoria_Producto>().HasIndex(x => x.descripcion).IsUnique();
        modelBuilder.Entity<Producto>().HasIndex(x => x.codigo_producto).IsUnique();
        modelBuilder.Entity<Producto>().HasIndex(x => x.codigo_barras).IsUnique();
        modelBuilder.Entity<Tarifa_IVA>().HasIndex(x => x.tarifa_iva_id).IsUnique();
        modelBuilder.Entity<Tarifa_IVA>().HasIndex(x => x.codigo_Iva).IsUnique();
        modelBuilder.Entity<Usuario>().HasIndex(x => x.nombre_usuario).IsUnique();
        modelBuilder.Entity<Usuario>().HasIndex(x => x.email).IsUnique();

        // Configurar propiedades adicionales si es necesario

        modelBuilder.Entity<Tarifa_IVA>().Property(t => t.porcentaje).HasPrecision(5, 2);
    }
}
