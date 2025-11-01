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
    public DbSet<TiposIdentificacion> TiposIdentificacions { get; set; }
    public DbSet<Tercero> Terceros { get; set; }
    public DbSet<TipoDcto> tipoDctos { get; set; }
    public DbSet<Consecutivo> consecutivos { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<Detalle_Factura> DetallesFactura { get; set; }
    public DbSet<Metodos_Pago> MetodosPago { get; set; }
    public DbSet<Pago_Factura> PagosFactura { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configurar índices únicos
        modelBuilder.Entity<Rol>().HasIndex(x => x.nombre).IsUnique();
        modelBuilder.Entity<Categoria_Producto>().HasIndex(x => x.descripcion).IsUnique();
        modelBuilder.Entity<Producto>().HasIndex(x => x.codigo_producto).IsUnique();
        modelBuilder.Entity<Producto>().HasIndex(x => x.codigo_barras).IsUnique();
        modelBuilder.Entity<Tarifa_IVA>().HasIndex(x => x.tarifa_iva_id).IsUnique();
        modelBuilder.Entity<Tarifa_IVA>().HasIndex(x => x.codigo_Iva).IsUnique();
        modelBuilder.Entity<Usuario>().HasIndex(x => x.nombre_usuario).IsUnique();
        modelBuilder.Entity<Usuario>().HasIndex(x => x.email).IsUnique();
        modelBuilder.Entity<TiposIdentificacion>().HasIndex(x => x.ID).IsUnique();
        modelBuilder.Entity<TiposIdentificacion>().HasIndex(x => x.tipoDocumentoID).IsUnique();
        modelBuilder.Entity<Tarifa_IVA>().Property(t => t.porcentaje).HasPrecision(5, 2);
        modelBuilder.Entity<Tercero>().HasIndex(x => x.tercero_id).IsUnique();
        modelBuilder.Entity<Tercero>().HasIndex(x => x.numero_identificacion).IsUnique();
        modelBuilder.Entity<TipoDcto>().HasIndex(x => x.Codigo).IsUnique();
        modelBuilder.Entity<Consecutivo>().HasIndex(x => x.cod_consecut).IsUnique();
        modelBuilder.Entity<Movimiento>().HasIndex(x => x.movimiento_id).IsUnique();
        modelBuilder.Entity<Movimiento>().HasIndex(x => x.numero_documento).IsUnique();
        modelBuilder.Entity<Factura>().HasIndex(x => x.factura_id).IsUnique();
        modelBuilder.Entity<Detalle_Factura>().HasIndex(x => x.detalle_id).IsUnique();
        modelBuilder.Entity<Metodos_Pago>().HasIndex(x => x.id_metodo_pago).IsUnique();
        modelBuilder.Entity<Metodos_Pago>().HasIndex(x => x.codigo_metpag).IsUnique();
        modelBuilder.Entity<Pago_Factura>().HasIndex(x => x.pago_id).IsUnique();

        // Configurar relaciones de Movimiento para evitar cascadas múltiples
        modelBuilder.Entity<Movimiento>()
            .HasOne(m => m.TipoDcto)
            .WithMany()
            .HasForeignKey(m => m.FK_codigo_tipodoc)
            .OnDelete(DeleteBehavior.Restrict); // Cambiar a Restrict

        modelBuilder.Entity<Movimiento>()
            .HasOne(m => m.Consecutivo)
            .WithMany()
            .HasForeignKey(m => m.FK_consecutivo_id)
            .OnDelete(DeleteBehavior.Restrict); // Cambiar a Restrict

        modelBuilder.Entity<Movimiento>()
            .HasOne(m => m.Usuario)
            .WithMany()
            .HasForeignKey(m => m.FK_usuario_id)
            .OnDelete(DeleteBehavior.Restrict); // Cambiar a Restrict

        modelBuilder.Entity<Movimiento>()
            .HasOne(m => m.Tercero)
            .WithMany()
            .HasForeignKey(m => m.FK_tercero_id)
            .OnDelete(DeleteBehavior.Restrict); // Cambiar a Restrict

        // Configurar relación de Factura con Movimiento
        modelBuilder.Entity<Factura>()
            .HasOne(f => f.Movimiento)
            .WithMany(m => m.Facturas)
            .HasForeignKey(f => f.FK_movimiento_id)
            .OnDelete(DeleteBehavior.Cascade); // Esta puede quedarse en Cascade

        // Configurar relaciones de Detalle_Factura para evitar cascadas múltiples
        modelBuilder.Entity<Detalle_Factura>()
            .HasOne(d => d.Factura)
            .WithMany(f => f.DetallesFactura)
            .HasForeignKey(d => d.FK_factura_id)
            .OnDelete(DeleteBehavior.Cascade); // Cascade para eliminar detalles al eliminar factura

        modelBuilder.Entity<Detalle_Factura>()
            .HasOne(d => d.Producto)
            .WithMany()
            .HasForeignKey(d => d.FK_producto_id)
            .OnDelete(DeleteBehavior.Restrict); // Restrict para no eliminar producto si tiene detalles

        // Configurar relaciones de Pago_Factura para evitar cascadas múltiples
        modelBuilder.Entity<Pago_Factura>()
            .HasOne(p => p.Factura)
            .WithMany(f => f.PagosFactura)
            .HasForeignKey(p => p.FK_factura_id)
            .OnDelete(DeleteBehavior.Cascade); // Cascade para eliminar pagos al eliminar factura

        modelBuilder.Entity<Pago_Factura>()
            .HasOne(p => p.MetodoPago)
            .WithMany(m => m.PagosFactura)
            .HasForeignKey(p => p.FK_id_metodo_pago)
            .OnDelete(DeleteBehavior.Restrict); // Restrict para no eliminar método de pago si tiene pagos
    }
}
