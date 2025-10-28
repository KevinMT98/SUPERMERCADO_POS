using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Data;

/// <summary>
/// Clase para crear la base de datos y poblar datos iniciales (seed data)
/// </summary>
public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Ejecuta el proceso de población de datos iniciales
    /// </summary>
    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckRolsAsync();
        await CheckCategoria_ProductosAsync();
        await CheckTarifa_IVAsAsync();
        await CheckUsuariosAsync();
        await CheckProductosAsync();
        await CheckTipoDctoAsync();
    }

    /// <summary>
    /// Verifica y crea categorías de productos iniciales
    /// </summary>
    private async Task CheckCategoria_ProductosAsync()
    {
        if (!_context.Categoria_Productos.Any())
        {
            _context.Categoria_Productos.Add(new Categoria_Producto { descripcion = "Bebidas", activo = true });
            _context.Categoria_Productos.Add(new Categoria_Producto { descripcion = "Lacteos", activo = true });
            _context.Categoria_Productos.Add(new Categoria_Producto { descripcion = "Aseo", activo = true });
            _context.Categoria_Productos.Add(new Categoria_Producto { descripcion = "Comida", activo = true });
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Verifica y crea roles iniciales del sistema
    /// </summary>
    private async Task CheckRolsAsync()
    {
        if (!_context.Rols.Any())
        {
            _context.Rols.Add(new Rol { nombre = "Admin", activo = true});
            _context.Rols.Add(new Rol { nombre = "User", activo = true });
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckTipoDctoAsync()
    {
        if (!_context.tipoDctos.Any())
        {
            _context.tipoDctos.Add(new TipoDcto { Codigo = "FA", Descripcion = "Factura Venta"});
            _context.tipoDctos.Add(new TipoDcto { Codigo = "NC", Descripcion = "Nota Credito" });
            _context.tipoDctos.Add(new TipoDcto { Codigo = "ND", Descripcion = "Nota Debito"});
            _context.tipoDctos.Add(new TipoDcto { Codigo = "PD", Descripcion = "Pedidos"});
            await _context.SaveChangesAsync();
        }
    }
    /// <summary>
    /// Verifica y crea usuarios iniciales del sistema
    /// </summary>
    private async Task CheckUsuariosAsync()
    {
        if (!_context.Usuarios.Any())
        {
            // Obtener los roles para asignar a los usuarios
            var rolAdmin = _context.Rols.FirstOrDefault(r => r.nombre == "Admin");
            var rolUser = _context.Rols.FirstOrDefault(r => r.nombre == "User");

            if (rolAdmin != null)
            {
                // Crear usuario administrador principal
                _context.Usuarios.Add(new Usuario
                {
                    nombre_usuario = "admin",
                    password_hash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    nombre = "Administrador",
                    apellido = "Sistema",
                    email = "admin@supermercado.com",
                    FK_rol_id = rolAdmin.rol_id,
                    activo = true,
                    fecha_creacion = DateTime.UtcNow
                });

                // Crear super administrador
                _context.Usuarios.Add(new Usuario
                {
                    nombre_usuario = "superadmin",
                    password_hash = BCrypt.Net.BCrypt.HashPassword("Super123!"),
                    nombre = "Super",
                    apellido = "Administrador",
                    email = "superadmin@supermercado.com",
                    FK_rol_id = rolAdmin.rol_id,
                    activo = true,
                    fecha_creacion = DateTime.UtcNow
                });
            }

            if (rolUser != null)
            {
                // Crear usuarios de prueba con rol User
                _context.Usuarios.Add(new Usuario
                {
                    nombre_usuario = "usuario1",
                    password_hash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                    nombre = "Juan",
                    apellido = "Pérez",
                    email = "juan.perez@supermercado.com",
                    FK_rol_id = rolUser.rol_id,
                    activo = true,
                    fecha_creacion = DateTime.UtcNow
                });

                _context.Usuarios.Add(new Usuario
                {
                    nombre_usuario = "usuario2",
                    password_hash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                    nombre = "María",
                    apellido = "García",
                    email = "maria.garcia@supermercado.com",
                    FK_rol_id = rolUser.rol_id,
                    activo = true,
                    fecha_creacion = DateTime.UtcNow
                });

                // Usuario inactivo para pruebas de autenticación
                _context.Usuarios.Add(new Usuario
                {
                    nombre_usuario = "usuario_inactivo",
                    password_hash = BCrypt.Net.BCrypt.HashPassword("Inactive123!"),
                    nombre = "Usuario",
                    apellido = "Inactivo",
                    email = "inactivo@supermercado.com",
                    FK_rol_id = rolUser.rol_id,
                    activo = false,
                    fecha_creacion = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Verifica y crea tarifas de IVA iniciales
    /// </summary>
    private async Task CheckTarifa_IVAsAsync()
    {
        if (!_context.Tarifa_IVAs.Any())
        {
            _context.Tarifa_IVAs.Add(new Tarifa_IVA 
            { 
                codigo_Iva = "IVA0", 
                descripcion = "Exento de IVA", 
                porcentaje = 0m, 
                estado = true 
            });
            _context.Tarifa_IVAs.Add(new Tarifa_IVA 
            { 
                codigo_Iva = "IVA5", 
                descripcion = "IVA Reducido", 
                porcentaje = 5m, 
                estado = true 
            });
            _context.Tarifa_IVAs.Add(new Tarifa_IVA 
            { 
                codigo_Iva = "IVA19", 
                descripcion = "IVA General", 
                porcentaje = 19m, 
                estado = true 
            });
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Verifica y crea productos de prueba iniciales
    /// </summary>
    private async Task CheckProductosAsync()
    {
        if (!_context.Productos.Any())
        {
            // Obtener las categorías y tarifas de IVA para las relaciones
            var categoriaBebidas = _context.Categoria_Productos.FirstOrDefault(c => c.descripcion == "Bebidas");
            var categoriaLacteos = _context.Categoria_Productos.FirstOrDefault(c => c.descripcion == "Lacteos");
            var categoriaAseo = _context.Categoria_Productos.FirstOrDefault(c => c.descripcion == "Aseo");
            var categoriaComida = _context.Categoria_Productos.FirstOrDefault(c => c.descripcion == "Comida");
            
            var iva19 = _context.Tarifa_IVAs.FirstOrDefault(i => i.codigo_Iva == "IVA19");
            var iva5 = _context.Tarifa_IVAs.FirstOrDefault(i => i.codigo_Iva == "IVA5");
            var iva0 = _context.Tarifa_IVAs.FirstOrDefault(i => i.codigo_Iva == "IVA0");

            if (categoriaBebidas != null && iva19 != null)
            {
                _context.Productos.Add(new Producto
                {
                    codigo_producto = "BEB001",
                    codigo_barras = "7702001234567",
                    nombre = "Coca Cola 2L",
                    descripcion = "Bebida gaseosa sabor cola 2 litros",
                    precio_unitario = 5500m,
                    FK_categoria_id = categoriaBebidas.categoriaId,
                    FK_codigo_iva = iva19.tarifa_iva_id,
                    stock_actual = 50,
                    stock_minimo = 10,
                    stock_maximo = 100,
                    activo = true
                });

                _context.Productos.Add(new Producto
                {
                    codigo_producto = "BEB002",
                    codigo_barras = "7702001234574",
                    nombre = "Agua Cristal 600ml",
                    descripcion = "Agua natural sin gas",
                    precio_unitario = 1500m,
                    FK_categoria_id = categoriaBebidas.categoriaId,
                    FK_codigo_iva = iva19.tarifa_iva_id,
                    stock_actual = 100,
                    stock_minimo = 20,
                    stock_maximo = 200,
                    activo = true
                });
            }

            if (categoriaLacteos != null && iva5 != null)
            {
                _context.Productos.Add(new Producto
                {
                    codigo_producto = "LAC001",
                    codigo_barras = "7702002234567",
                    nombre = "Leche Entera 1L",
                    descripcion = "Leche entera pasteurizada",
                    precio_unitario = 3200m,
                    FK_categoria_id = categoriaLacteos.categoriaId,
                    FK_codigo_iva = iva5.tarifa_iva_id,
                    stock_actual = 30,
                    stock_minimo = 15,
                    stock_maximo = 80,
                    activo = true
                });
            }

            if (categoriaAseo != null && iva19 != null)
            {
                _context.Productos.Add(new Producto
                {
                    codigo_producto = "ASE001",
                    codigo_barras = "7702003234567",
                    nombre = "Jabón Líquido 500ml",
                    descripcion = "Jabón líquido antibacterial",
                    precio_unitario = 8500m,
                    FK_categoria_id = categoriaAseo.categoriaId,
                    FK_codigo_iva = iva19.tarifa_iva_id,
                    stock_actual = 25,
                    stock_minimo = 5,
                    stock_maximo = 50,
                    activo = true
                });
            }

            if (categoriaComida != null && iva0 != null)
            {
                _context.Productos.Add(new Producto
                {
                    codigo_producto = "COM001",
                    codigo_barras = "7702004234567",
                    nombre = "Arroz Blanco 1Kg",
                    descripcion = "Arroz blanco de primera calidad",
                    precio_unitario = 3500m,
                    FK_categoria_id = categoriaComida.categoriaId,
                    FK_codigo_iva = iva0.tarifa_iva_id,
                    stock_actual = 60,
                    stock_minimo = 20,
                    stock_maximo = 150,
                    activo = true
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
