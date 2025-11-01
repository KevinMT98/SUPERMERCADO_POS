using Microsoft.EntityFrameworkCore;
using Supermercado.Shared.Entities;

namespace Supermercado.Backend.Data;

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
        await CheckTiposIdentificacionAsync();
        await CheckCategoria_ProductosAsync();
        await CheckTarifa_IVAsAsync();
        await CheckTipoDctoAsync();
        await CheckConsecutAsync();
        await CheckMetodosPagoAsync();
        await CheckUsuariosAsync();
        await CheckTercerosAsync();
        await CheckProductosAsync();
    }

    private async Task CheckRolsAsync()
    {
        if (!_context.Rols.Any())
        {
            _context.Rols.Add(new Rol { nombre = "Admin", activo = true });
            _context.Rols.Add(new Rol { nombre = "User", activo = true });
            await _context.SaveChangesAsync();
        }
    }

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

    private async Task CheckTarifa_IVAsAsync()
    {
        if (!_context.Tarifa_IVAs.Any())
        {
            _context.Tarifa_IVAs.Add(new Tarifa_IVA { codigo_Iva = "IVA0", descripcion = "Exento de IVA", porcentaje = 0m, estado = true });
            _context.Tarifa_IVAs.Add(new Tarifa_IVA { codigo_Iva = "IVA5", descripcion = "IVA Reducido", porcentaje = 5m, estado = true });
            _context.Tarifa_IVAs.Add(new Tarifa_IVA { codigo_Iva = "IVA19", descripcion = "IVA General", porcentaje = 19m, estado = true });
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckTipoDctoAsync()
    {
        if (!_context.tipoDctos.Any())
        {
            _context.tipoDctos.Add(new TipoDcto { Codigo = "FA", Descripcion = "Factura Venta" });
            _context.tipoDctos.Add(new TipoDcto { Codigo = "NC", Descripcion = "Nota Credito" });
            _context.tipoDctos.Add(new TipoDcto { Codigo = "ND", Descripcion = "Nota Debito" });
            _context.tipoDctos.Add(new TipoDcto { Codigo = "PD", Descripcion = "Pedidos" });
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckConsecutAsync()
    {
        if (!_context.consecutivos.Any())
        {
            var tipoFA = _context.tipoDctos.FirstOrDefault(t => t.Codigo == "FA");
            if (tipoFA != null)
            {
                _context.consecutivos.Add(new Consecutivo
                {
                    cod_consecut = "FV",
                    descripcion = "Consecutivo para Facturas de Venta",
                    consecutivo_ini = 1,
                    consecutivo_fin = 100000,
                    consecutivo_actual = 0,
                    FK_codigo_tipodcto = tipoFA.ID,
                    afecta_inv = true,
                    es_entrada = false
                });
                await _context.SaveChangesAsync();
            }
        }
    }

    private async Task CheckMetodosPagoAsync()
    {
        if (!_context.MetodosPago.Any())
        {
            _context.MetodosPago.Add(new Metodos_Pago { codigo_metpag = "EF", metodo_pago = "Efectivo", activo = true });
            _context.MetodosPago.Add(new Metodos_Pago { codigo_metpag = "TC", metodo_pago = "Tarjeta de Crédito", activo = true });
            _context.MetodosPago.Add(new Metodos_Pago { codigo_metpag = "TD", metodo_pago = "Tarjeta de Débito", activo = true });
            _context.MetodosPago.Add(new Metodos_Pago { codigo_metpag = "TR", metodo_pago = "Transferencia Bancaria", activo = true });
            _context.MetodosPago.Add(new Metodos_Pago { codigo_metpag = "NQ", metodo_pago = "Nequi", activo = true });
            _context.MetodosPago.Add(new Metodos_Pago { codigo_metpag = "DV", metodo_pago = "Daviplata", activo = true });
            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckUsuariosAsync()
    {
        if (!_context.Usuarios.Any())
        {
            var rolAdmin = _context.Rols.FirstOrDefault(r => r.nombre == "Admin");
            var rolUser = _context.Rols.FirstOrDefault(r => r.nombre == "User");

            if (rolAdmin != null)
            {
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

    private async Task CheckProductosAsync()
    {
        if (!_context.Productos.Any())
        {
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

    private async Task CheckTiposIdentificacionAsync()
    {
        if (!_context.TiposIdentificacions.Any())
        {
            _context.TiposIdentificacions.Add(new TiposIdentificacion
            {
                tipoDocumentoID = "CC",
                descripcion = "Cédula de Ciudadanía",
                activo = true
            });

            _context.TiposIdentificacions.Add(new TiposIdentificacion
            {
                tipoDocumentoID = "CE",
                descripcion = "Cédula de Extranjería",
                activo = true
            });

            _context.TiposIdentificacions.Add(new TiposIdentificacion
            {
                tipoDocumentoID = "NIT",
                descripcion = "NIT",
                activo = true
            });

            _context.TiposIdentificacions.Add(new TiposIdentificacion
            {
                tipoDocumentoID = "PAS",
                descripcion = "Pasaporte",
                activo = true
            });

            await _context.SaveChangesAsync();
        }
    }

    private async Task CheckTercerosAsync()
    {
        if (!_context.Terceros.Any())
        {
            var tipoCC = _context.TiposIdentificacions.FirstOrDefault(t => t.tipoDocumentoID == "CC");
            var tipoNIT = _context.TiposIdentificacions.FirstOrDefault(t => t.tipoDocumentoID == "NIT");

            if (tipoCC != null)
            {
                // Cliente 1
                _context.Terceros.Add(new Tercero
                {
                    FK_codigo_ident = tipoCC.ID,
                    numero_identificacion = "1234567890",
                    nombre = "Carlos",
                    nombre2 = "Alberto",
                    apellido1 = "Gómez",
                    apellido2 = "Pérez",
                    email = "carlos.gomez@email.com",
                    direccion = "Calle 123 #45-67",
                    telefono = "3001234567",
                    es_cliente = true,
                    es_proveedor = false,
                    activo = true
                });

                // Cliente 2
                _context.Terceros.Add(new Tercero
                {
                    FK_codigo_ident = tipoCC.ID,
                    numero_identificacion = "9876543210",
                    nombre = "Ana",
                    nombre2 = "María",
                    apellido1 = "Rodríguez",
                    apellido2 = "López",
                    email = "ana.rodriguez@email.com",
                    direccion = "Carrera 45 #12-34",
                    telefono = "3109876543",
                    es_cliente = true,
                    es_proveedor = false,
                    activo = true
                });

                // Cliente 3
                _context.Terceros.Add(new Tercero
                {
                    FK_codigo_ident = tipoCC.ID,
                    numero_identificacion = "5555555555",
                    nombre = "Luis",
                    nombre2 = "",
                    apellido1 = "Martínez",
                    apellido2 = "Castro",
                    email = "luis.martinez@email.com",
                    direccion = "Avenida 68 #100-25",
                    telefono = "3205555555",
                    es_cliente = true,
                    es_proveedor = false,
                    activo = true
                });
            }

            if (tipoNIT != null)
            {
                // Proveedor 1
                _context.Terceros.Add(new Tercero
                {
                    FK_codigo_ident = tipoNIT.ID,
                    numero_identificacion = "900123456-1",
                    nombre = "Distribuidora",
                    nombre2 = "La",
                    apellido1 = "Economía",
                    apellido2 = "SAS",
                    email = "ventas@laeconomia.com",
                    direccion = "Calle 50 #20-30",
                    telefono = "6012345678",
                    es_cliente = false,
                    es_proveedor = true,
                    activo = true
                });

                // Cliente y Proveedor
                _context.Terceros.Add(new Tercero
                {
                    FK_codigo_ident = tipoNIT.ID,
                    numero_identificacion = "800987654-3",
                    nombre = "Supermercados",
                    nombre2 = "El",
                    apellido1 = "Ahorro",
                    apellido2 = "LTDA",
                    email = "contacto@elahorro.com",
                    direccion = "Carrera 7 #32-16",
                    telefono = "6019876543",
                    es_cliente = true,
                    es_proveedor = true,
                    activo = true
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
