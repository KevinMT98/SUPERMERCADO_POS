# ?? Sistema POS para Supermercado

<div align="center">

![.NET Version](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet)
![C# Version](https://img.shields.io/badge/C%23-13.0-239120?style=for-the-badge&logo=csharp)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-Core-512BD4?style=for-the-badge&logo=nuget)
![License](https://img.shields.io/badge/License-MIT-green?style=for-the-badge)

**Sistema de Punto de Venta (POS) moderno y escalable desarrollado con .NET 9**

[Caracter�sticas](#-caracter�sticas-principales) � [Tecnolog�as](#-tecnolog�as-utilizadas) � [Arquitectura](#-arquitectura-del-sistema) � [Instalaci�n](#-instalaci�n) � [Documentaci�n](#-documentaci�n)

</div>

---

## ?? Tabla de Contenidos

- [Introducci�n](#-introducci�n)
- [Objetivo General](#-objetivo-general)
- [Objetivos Espec�ficos](#-objetivos-espec�ficos)
- [Alcance del Proyecto](#-alcance-del-proyecto)
- [Plan de Trabajo](#-plan-de-trabajo)
- [Caracter�sticas Principales](#-caracter�sticas-principales)
- [Tecnolog�as Utilizadas](#-tecnolog�as-utilizadas)
- [Manejo de ORM y Documentaci�n de API](#️-manejo-de-orm-y-documentaci�n-de-api)
- [Arquitectura del Sistema](#-arquitectura-del-sistema)
- [Diagrama de Clases](#-diagrama-de-clases)
- [Estructura del Proyecto](#-estructura-del-proyecto)
- [Principios de Desarrollo](#-principios-de-desarrollo)
- [Instalaci�n](#-instalaci�n)
- [Documentaci�n](#-documentaci�n)
- [Autores](#-autores)

---

## ?? Introducci�n

El **Sistema POS para Supermercado** es una soluci�n integral desarrollada para modernizar y optimizar las operaciones de venta en establecimientos comerciales. Este sistema implementa las mejores pr�cticas de desarrollo de software, utilizando .NET 9 y siguiendo una arquitectura limpia y escalable basada en los principios SOLID y patrones de dise�o reconocidos en la industria.

El proyecto surge de la necesidad de contar con una herramienta robusta que permita:
- **Gesti�n eficiente** de inventarios en tiempo real
- **Facturaci�n electr�nica** con c�lculos autom�ticos de impuestos y descuentos
- **Control de usuarios** y roles con seguridad incorporada
- **Reportes y an�lisis** de ventas para la toma de decisiones
- **Integraci�n flexible** con diferentes m�todos de pago

Este sistema est� dise�ado para ser **mantenible, escalable y f�cil de extender**, permitiendo adaptarse a las necesidades cambiantes del negocio.

---

## ?? Objetivo General

Desarrollar un **sistema de punto de venta (POS) robusto y escalable** para supermercados, que permita gestionar de manera eficiente las operaciones de ventas, inventario, facturaci�n y control de usuarios, implementando las mejores pr�cticas de desarrollo de software con .NET 9, siguiendo los principios SOLID y una arquitectura limpia que garantice la mantenibilidad, escalabilidad y calidad del c�digo.

---

## ?? Objetivos Espec�ficos

### 1. ?? Implementar un M�dulo de Gesti�n de Inventario
- Desarrollar funcionalidades CRUD completas para productos, categor�as y proveedores
- Implementar control de stock con alertas de niveles m�nimos y m�ximos
- Gestionar tarifas de IVA asociadas a cada producto
- Proveer validaciones de negocio para garantizar la integridad de los datos

### 2. ?? Desarrollar un Sistema de Facturaci�n Integral
- Implementar proceso de facturaci�n con c�lculos autom�ticos de:
  - Descuentos (por porcentaje o valor fijo)
  - Impuestos (IVA) sobre base gravable
  - Totales y subtotales
- Integrar m�ltiples m�todos de pago (efectivo, tarjetas, transferencias)
- Generar consecutivos autom�ticos de facturaci�n
- Validar disponibilidad de stock en tiempo real

### 3. ?? Crear un Sistema de Gesti�n de Usuarios y Seguridad
- Implementar autenticaci�n y autorizaci�n basada en roles
- Gestionar permisos granulares por funcionalidad
- Registrar auditor�a de operaciones cr�ticas
- Proteger contrase�as con algoritmos de hashing seguros (BCrypt)

### 4. ??? Establecer una Arquitectura S�lida y Escalable
- Aplicar arquitectura en capas (Repository Pattern, Unit of Work)
- Implementar los 5 principios SOLID en todo el c�digo
- Utilizar inyecci�n de dependencias para desacoplamiento
- Implementar manejo centralizado de errores y validaciones

### 5. ?? Desarrollar M�dulo de Reportes y An�lisis
- Generar reportes de ventas por fecha, usuario y m�todo de pago
- Implementar filtros avanzados para consultas de facturas
- Proveer res�menes de ventas diarias
- Identificar facturas pendientes de pago

---

## ?? Alcance del Proyecto

### ? Incluido en el Alcance Inicial

#### **M�dulos Principales:**
1. **Gesti�n de Productos**
   - CRUD de productos con c�digo de barras
   - Gesti�n de categor�as de productos
   - Control de stock (actual, m�nimo, m�ximo)
   - Asociaci�n de tarifas de IVA

2. **Gesti�n de Terceros**
   - Registro de clientes y proveedores
   - Tipos de identificaci�n (CC, NIT, CE, Pasaporte)
   - Informaci�n de contacto completa

3. **Sistema de Facturaci�n**
   - Creaci�n de facturas de venta
   - Detalles de factura con productos
   - M�ltiples m�todos de pago por factura
   - C�lculos autom�ticos (descuentos, IVA, totales)
   - Anulaci�n de facturas con auditor�a

4. **Gesti�n de Usuarios y Seguridad**
   - Autenticaci�n y autorizaci�n
   - Roles (Admin, User)
   - Cifrado de contrase�as

5. **Configuraci�n del Sistema**
   - Tipos de documentos
   - Consecutivos autom�ticos
   - M�todos de pago
   - Tarifas de IVA

#### **Funcionalidades T�cnicas:**
- API RESTful completa
- Validaciones de negocio exhaustivas
- Transacciones de base de datos (ACID)
- DTOs para transferencia de datos
- AutoMapper para mapeo de objetos
- Helpers para c�lculos reutilizables

### ? Fuera del Alcance Inicial

- Interfaz gr�fica de usuario (Frontend)
- Integraci�n con facturaci�n electr�nica DIAN
- Sistema de compras y �rdenes de compra
- Gesti�n de empleados y n�mina
- Integraci�n con terminales de punto de venta f�sicos
- Reportes avanzados con gr�ficos
- Sistema de CRM (Customer Relationship Management)
- Aplicaci�n m�vil

---

## ?? Plan de Trabajo

### **Fase 1: An�lisis y Dise�o (2 semanas)**
- ? Definici�n de requisitos funcionales y no funcionales
- ? Dise�o de la base de datos (diagrama ER)
- ? Dise�o de arquitectura del sistema
- ? Definici�n de DTOs y contratos de API
- ? Documentaci�n de principios SOLID aplicados

### **Fase 2: Configuraci�n del Proyecto (1 semana)**
- ? Creaci�n de proyectos .NET 9
- ? Configuraci�n de Entity Framework Core
- ? Configuraci�n de AutoMapper
- ? Configuraci�n de inyecci�n de dependencias
- ? Configuraci�n de Swagger/OpenAPI

### **Fase 3: Desarrollo del Backend - M�dulos Base (3 semanas)**
- ? Implementaci�n de entidades y DbContext
- ? Implementaci�n de Repository Pattern
- ? Implementaci�n de Unit of Work Pattern
- ? Desarrollo de m�dulos:
  - ? Gesti�n de Usuarios
  - ? Gesti�n de Roles
  - ? Gesti�n de Tipos de Identificaci�n
  - ? Gesti�n de Terceros
  - ? Gesti�n de Categor�as de Productos

### **Fase 4: Desarrollo del Backend - M�dulos de Inventario (2 semanas)**
- ? Gesti�n de Tarifas de IVA
- ? Gesti�n de Productos
- ? Control de Stock
- ? Validaciones de negocio

### **Fase 5: Desarrollo del Backend - Sistema de Facturaci�n (3 semanas)**
- ✅ Gesti�n de Tipos de Documentos
- ✅ Gesti�n de Consecutivos
- ✅ Gesti�n de M�todos de Pago
- ✅ Gesti�n de Movimientos
- ✅ Gesti�n de Facturas
- ✅ Detalles de Factura
- ✅ Pagos de Factura
- ✅ **FacturacionRepository** (proceso completo)
- ✅ C�lculos autom�ticos (descuentos, **IVA**, totales)
- ✅ Validaciones exhaustivas
- ✅ Anulaci�n de facturas
- ✅ **Integraci�n completa con tarifas de IVA**
- ✅ **Transacciones at�micas con Entity Framework**

### **Fase 6: Helpers y Utilidades (1 semana)**
- ? FacturacionHelper para c�lculos
- ? M�todos de validaci�n reutilizables
- ? Formateo de datos

### **Fase 7: Pruebas y Validaci�n (2 semanas)**
- ?? Pruebas unitarias de repositorios
- ?? Pruebas de integraci�n
- ?? Validaci�n de reglas de negocio
- ?? Pruebas de API con Postman/Swagger

### **Fase 8: Documentaci�n y Entrega (1 semana)**
- ? Documentaci�n de c�digo (XML Comments)
- ? Documentaci�n de facturaci�n
- ? Documentaci�n de principios SOLID
- ? README del proyecto
- ?? Manual de usuario de API

**Duraci�n Total Estimada:** 15 semanas

---

## ? Caracter�sticas Principales

### ?? **Seguridad Robusta**
- Autenticaci�n basada en usuarios y roles
- Cifrado de contrase�as con BCrypt
- Validaci�n de permisos a nivel de operaci�n
- Auditor�a de operaciones cr�ticas

### ?? **Gesti�n de Inventario Inteligente**
- Control de stock en tiempo real
- Alertas de stock m�nimo y m�ximo
- Gesti�n de categor�as de productos
- Asociaci�n de tarifas de IVA por producto

### ?? **Facturaci�n Completa**
- C�lculos autom�ticos de descuentos e **impuestos (IVA)**
- Soporte para m�ltiples m�todos de pago
- Consecutivos autom�ticos de facturaci�n
- Anulaci�n de facturas con trazabilidad
- Validaci�n de stock antes de facturar
- **Integraci�n completa con Entity Framework ORM**
- **Documentaci�n interactiva con Swagger/OpenAPI**

### ?? **Reportes y Consultas**
- Resumen de ventas por fecha
- Consultas con filtros avanzados
- Facturas pendientes de pago
- Ventas por m�todo de pago

### ??? **Arquitectura Profesional**
- Implementaci�n de principios SOLID
- Repository Pattern y Unit of Work
- Inyecci�n de dependencias
- API RESTful bien documentada
- Manejo centralizado de errores

---

## ??? Tecnolog�as Utilizadas

### **Backend**
| Tecnolog�a | Versi�n | Prop�sito |
|------------|---------|-----------|
| **.NET** | 9.0 | Framework principal |
| **C#** | 13.0 | Lenguaje de programaci�n |
| **Entity Framework Core** | 9.0 | ORM para acceso a datos |
| **SQL Server** | 2022 | Base de datos |
| **AutoMapper** | 13.0 | Mapeo de objetos |
| **BCrypt.Net** | Latest | Hash de contrase�as |
| **Swagger/OpenAPI** | Latest | Documentaci�n de API |

### **Patrones y Pr�cticas**
- ? **Repository Pattern** - Abstracci�n de acceso a datos
- ? **Unit of Work Pattern** - Coordinaci�n de transacciones
- ? **Dependency Injection** - Desacoplamiento de dependencias
- ? **DTO Pattern** - Transferencia de datos
- ? **SOLID Principles** - C�digo limpio y mantenible
- ? **Clean Architecture** - Separaci�n de responsabilidades

---

## 🗄️ Manejo de ORM y Documentación de API

### **Entity Framework Core - ORM**

#### **Configuración del Contexto de Datos**
El sistema utiliza **Entity Framework Core 9.0** como ORM principal para el manejo de la base de datos:

```csharp
public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    // DbSets para todas las entidades
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Tercero> Terceros { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Factura> Facturas { get; set; }
    public DbSet<Detalle_Factura> DetallesFactura { get; set; }
    public DbSet<Pago_Factura> PagosFactura { get; set; }
    public DbSet<Movimiento> Movimientos { get; set; }
    // ... más entidades
}
```

#### **Configuración de Conexión**
```csharp
// Program.cs - Configuración de Entity Framework
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
```

#### **Características del ORM Implementadas**

✅ **Mapeo de Entidades Completo**
- 15+ entidades mapeadas con relaciones complejas
- Configuración de claves foráneas y navegación
- Validaciones a nivel de entidad con Data Annotations

✅ **Relaciones y Navegación**
```csharp
// Ejemplo: Factura con navegación a detalles y pagos
public class Factura
{
    [Key]
    public int factura_id { get; set; }
    
    [ForeignKey(nameof(FK_movimiento_id))]
    public Movimiento? Movimiento { get; set; }
    
    public ICollection<Detalle_Factura>? DetallesFactura { get; set; }
    public ICollection<Pago_Factura>? PagosFactura { get; set; }
}
```

✅ **Consultas Complejas con LINQ**
```csharp
// Consulta con múltiples includes para facturación
var factura = await _context.Facturas
    .Include(f => f.Movimiento)
        .ThenInclude(m => m!.Tercero)
    .Include(f => f.Movimiento)
        .ThenInclude(m => m!.Usuario)
    .Include(f => f.DetallesFactura)
        .ThenInclude(d => d!.Producto)
            .ThenInclude(p => p!.TarifaIVA)
    .Include(f => f.PagosFactura)
        .ThenInclude(p => p!.MetodoPago)
    .FirstOrDefaultAsync(f => f.factura_id == facturaId);
```

✅ **Transacciones Atómicas**
```csharp
// Manejo de transacciones en operaciones complejas
using var transaction = await _context.Database.BeginTransactionAsync();
try
{
    // Múltiples operaciones
    _context.Movimientos.Add(movimiento);
    await _context.SaveChangesAsync();
    
    _context.Facturas.Add(factura);
    await _context.SaveChangesAsync();
    
    await transaction.CommitAsync();
}
catch (Exception ex)
{
    await transaction.RollbackAsync();
    throw;
}
```

✅ **Migraciones y Seeding**
- Configuración automática de base de datos
- Datos iniciales (seed data) para desarrollo
- Manejo de cambios de esquema

#### **Ventajas del ORM en el Proyecto**

| Ventaja | Implementación |
|---------|----------------|
| **Type Safety** | Consultas tipadas con LINQ |
| **Lazy Loading** | Navegación automática entre entidades |
| **Change Tracking** | Detección automática de cambios |
| **Migrations** | Versionado de esquema de BD |
| **Connection Pooling** | Optimización de conexiones |
| **SQL Injection Prevention** | Consultas parametrizadas automáticas |

---

### **Swagger/OpenAPI - Documentación de API**

#### **Configuración de Swagger**
```csharp
// Program.cs - Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Supermercado POS API", 
        Version = "v1",
        Description = "API para Sistema de Punto de Venta",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo",
            Email = "desarrollo@supermercado.com"
        }
    });
    
    // Incluir comentarios XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    
    // Configuración de JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });
});

// Habilitar Swagger en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Supermercado POS API v1");
        c.RoutePrefix = string.Empty; // Swagger en la raíz
    });
}
```

#### **Documentación de Endpoints**

✅ **Comentarios XML Detallados**
```csharp
/// <summary>
/// Crea una factura completa con detalles y pagos
/// </summary>
/// <param name="facturaDto">Datos de la factura a crear</param>
/// <returns>Factura creada con todos sus detalles</returns>
/// <response code="201">Factura creada exitosamente</response>
/// <response code="400">Datos inválidos o error de validación</response>
/// <response code="401">No autorizado</response>
[HttpPost("crear-factura")]
[ProducesResponseType(typeof(FacturaCompletaDTO), StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public async Task<IActionResult> CrearFacturaCompletaAsync([FromBody] FacturaCompletaCreateDTO facturaDto)
```

✅ **Esquemas de Datos Documentados**
```csharp
/// <summary>
/// DTO para crear una factura completa con todos sus detalles y pagos
/// </summary>
public class FacturaCompletaCreateDTO
{
    /// <summary>
    /// ID del tercero (cliente)
    /// </summary>
    /// <example>1</example>
    [Required(ErrorMessage = "El tercero es obligatorio")]
    public int TerceroId { get; set; }

    /// <summary>
    /// Lista de productos a facturar
    /// </summary>
    [Required(ErrorMessage = "Los detalles de la factura son obligatorios")]
    [MinLength(1, ErrorMessage = "Debe incluir al menos un producto")]
    public List<DetalleFacturaItemDTO> Detalles { get; set; } = new();
}
```

#### **Características de Swagger Implementadas**

✅ **Interfaz Interactiva Completa**
- Pruebas en vivo de todos los endpoints
- Validación de esquemas en tiempo real
- Ejemplos de request/response automáticos

✅ **Autenticación JWT Integrada**
- Botón "Authorize" en la interfaz
- Headers de autorización automáticos
- Pruebas con tokens reales

✅ **Documentación Automática**
- Generación de esquemas desde DTOs
- Validaciones mostradas en la UI
- Códigos de respuesta documentados

✅ **Agrupación por Controladores**
```
📁 Auth - Autenticación y autorización
📁 Facturacion - Sistema completo de facturación
📁 Producto - Gestión de productos
📁 Tercero - Gestión de clientes/proveedores
📁 Usuario - Administración de usuarios
```

#### **Beneficios de Swagger en el Proyecto**

| Beneficio | Descripción |
|-----------|-------------|
| **Documentación Viva** | Se actualiza automáticamente con el código |
| **Testing Integrado** | Pruebas directas desde la interfaz web |
| **Validación Visual** | Esquemas y validaciones claramente mostrados |
| **Colaboración** | Fácil compartir con frontend y QA |
| **Estándares** | Cumple con especificación OpenAPI 3.0 |

#### **Acceso a la Documentación**

🌐 **URLs de Acceso:**
- **Swagger UI:** `http://localhost:5000/`
- **JSON Schema:** `http://localhost:5000/swagger/v1/swagger.json`
- **Redoc (alternativo):** `http://localhost:5000/redoc`

#### **Ejemplos de Uso desde Swagger**

**1. Autenticación:**
```json
POST /api/auth/login
{
  "email": "admin@supermercado.com",
  "password": "Admin123!"
}
```

**2. Crear Factura:**
```json
POST /api/facturacion/crear-factura
Authorization: Bearer {token}
{
  "terceroId": 1,
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2,
      "precioUnitario": 15000,
      "descuentoPorcentaje": 10
    }
  ],
  "pagos": [
    {
      "metodoPagoId": 1,
      "monto": 27000
    }
  ]
}
```

**3. Consultar Productos:**
```json
GET /api/facturacion/productos-disponibles
Authorization: Bearer {token}
```

---

## ??? Arquitectura del Sistema

### **Arquitectura en Capas**

```
???????????????????????????????????????????????????????????????
?                      API Layer                              ?
?                    (Controllers)                            ?
?  � FacturaController                                        ?
?  � ProductoController                                       ?
?  � UsuarioController                                        ?
?  � TerceroController                                        ?
???????????????????????????????????????????????????????????????
                         ?
                         ?
???????????????????????????????????????????????????????????????
?                  Business Logic Layer                       ?
?                   (Unit of Work)                            ?
?  � FacturacionUnitOfWork                                    ?
?  � ProductoUnitOfWork                                       ?
?  � UsuarioUnitOfWork                                        ?
?  � Validaciones de negocio                                  ?
???????????????????????????????????????????????????????????????
                         ?
                         ?
???????????????????????????????????????????????????????????????
?                  Data Access Layer                          ?
?                   (Repositories)                            ?
?  � FacturacionRepository                                    ?
?  � ProductoRepository                                       ?
?  � UsuarioRepository                                        ?
?  � GenericRepository<T>                                     ?
???????????????????????????????????????????????????????????????
                         ?
                         ?
???????????????????????????????????????????????????????????????
?                    Database Layer                           ?
?                  (Entity Framework Core)                    ?
?  � DataContext                                              ?
?  � Migrations                                               ?
?  � SQL Server Database                                      ?
???????????????????????????????????????????????????????????????

???????????????????????????????????????????????????????????????
?                    Cross-Cutting Concerns                   ?
?  � DTOs (Data Transfer Objects)                             ?
?  � Entities                                                 ?
?  � Helpers (FacturacionHelper)                              ?
?  � AutoMapper Profiles                                      ?
???????????????????????????????????????????????????????????????
```

### **Flujo de una Operaci�n de Facturaci�n**

```
1. Cliente HTTP Request (JSON)
          ?
2. FacturaController recibe FacturaCompletaCreateDTO
          ?
3. FacturacionUnitOfWork valida datos y reglas de negocio
          ?
4. FacturacionRepository ejecuta la l�gica de creaci�n
          ?
5. Transaction BEGIN
          ?
6. Validar Stock de Productos
          ?
7. Obtener Tipo de Documento y Consecutivo
          ?
8. Calcular Totales (Bruto, Descuentos, IVA, Neto)
          ?
9. Crear Movimiento
          ?
10. Crear Factura
          ?
11. Crear Detalles de Factura (con c�lculo de IVA)
          ?
12. Crear Pagos de Factura
          ?
13. Actualizar Stock de Productos
          ?
14. Actualizar Consecutivo
          ?
15. Transaction COMMIT
          ?
16. Obtener Factura Completa con relaciones
          ?
17. Mapear a FacturaCompletaDTO
          ?
18. Retornar ActionResponse<FacturaCompletaDTO>
          ?
19. HTTP Response (JSON)
```

---

## ?? Diagrama de Clases

### **Diagrama de Clases Principal - M�dulo de Facturaci�n**

```mermaid
classDiagram
    class Factura {
        +int factura_id
        +int FK_movimiento_id
        +decimal total_bruto
        +decimal total_descuentos
        +decimal total_impu
        +decimal total_neto
        +Movimiento Movimiento
        +ICollection~Detalle_Factura~ DetallesFactura
        +ICollection~Pago_Factura~ PagosFactura
    }

    class Movimiento {
        +int movimiento_id
        +int FK_codigo_tipodoc
        +int FK_consecutivo_id
        +string numero_documento
        +DateTime fecha
        +int FK_usuario_id
        +int FK_tercero_id
        +string observaciones
        +TipoDcto TipoDcto
        +Consecutivo Consecutivo
        +Usuario Usuario
        +Tercero Tercero
    }

    class Detalle_Factura {
        +int detalle_id
        +int FK_factura_id
        +int FK_producto_id
        +int cantidad
        +decimal precio_unitario
        +decimal descuento_porcentaje
        +decimal descuento_valor
        +decimal subtotal
        +Factura Factura
        +Producto Producto
    }

    class Pago_Factura {
        +int pago_id
        +int FK_factura_id
        +int FK_id_metodo_pago
        +decimal monto
        +string referencia_pago
        +Factura Factura
        +Metodos_Pago MetodoPago
    }

    class Producto {
        +int producto_id
        +string codigo_producto
        +string codigo_barras
        +string nombre
        +string descripcion
        +decimal precio_unitario
        +int FK_categoria_id
        +int FK_codigo_iva
        +int stock_actual
        +int stock_minimo
        +int stock_maximo
        +bool activo
        +Categoria_Producto Categoria
        +Tarifa_IVA TarifaIVA
    }

    class Tarifa_IVA {
        +int tarifa_iva_id
        +string codigo_Iva
        +string descripcion
        +decimal porcentaje
        +bool estado
    }

    class Tercero {
        +int tercero_id
        +int FK_codigo_ident
        +string numero_identificacion
        +string nombre
        +string apellido1
        +string email
        +string telefono
        +bool es_cliente
        +bool es_proveedor
        +bool activo
    }

    class Usuario {
        +int usuario_id
        +string nombre_usuario
        +string password_hash
        +string nombre
        +string apellido
        +string email
        +int FK_rol_id
        +bool activo
        +DateTime fecha_creacion
        +Rol Rol
    }

    class Metodos_Pago {
        +int id_metodo_pago
        +string codigo_metpag
        +string metodo_pago
        +bool activo
    }

    class TipoDcto {
        +int ID
        +string Codigo
        +string Descripcion
    }

    class Consecutivo {
        +int consecutivo_Id
        +string cod_consecut
        +int FK_codigo_tipodcto
        +string descripcion
        +int consecutivo_ini
        +int consecutivo_fin
        +int consecutivo_actual
        +bool afecta_inv
        +bool es_entrada
    }

    Factura "1" --> "1" Movimiento
    Factura "1" --> "*" Detalle_Factura
    Factura "1" --> "*" Pago_Factura
    Movimiento "*" --> "1" Usuario
    Movimiento "*" --> "1" Tercero
    Movimiento "*" --> "1" TipoDcto
    Movimiento "*" --> "1" Consecutivo
    Detalle_Factura "*" --> "1" Producto
    Producto "*" --> "1" Tarifa_IVA
    Pago_Factura "*" --> "1" Metodos_Pago
```

### **Diagrama de Clases - Arquitectura (Patrones)**

```mermaid
classDiagram
    class IFacturacionRepository {
        <<interface>>
        +CrearFacturaCompletaAsync()
        +ObtenerFacturaCompletaAsync()
        +CalcularTotalesFacturaAsync()
        +ValidarStockProductosAsync()
        +AnularFacturaAsync()
    }

    class FacturacionRepository {
        -DataContext _context
        +CrearFacturaCompletaAsync()
        +ObtenerFacturaCompletaAsync()
        +CalcularTotalesFacturaAsync()
        +ValidarStockProductosAsync()
        +ActualizarStockProductosAsync()
        +AnularFacturaAsync()
        -CalcularValoresDetalle()
    }

    class IFacturacionUnitOfWork {
        <<interface>>
        +CrearFacturaCompletaAsync()
        +ValidarDatosFacturaAsync()
        +ObtenerProductosDisponiblesAsync()
    }

    class FacturacionUnitOfWork {
        -IFacturacionRepository _repository
        -DataContext _context
        -IMapper _mapper
        +CrearFacturaCompletaAsync()
        +ValidarDatosFacturaAsync()
        +ValidarReglasNegocioAsync()
        +ObtenerProductosDisponiblesAsync()
    }

    class FacturaController {
        -IFacturacionUnitOfWork _unitOfWork
        -IMapper _mapper
        +PostAsync()
        +GetAsync()
        +DeleteAsync()
    }

    class FacturacionHelper {
        <<static>>
        +CalcularValoresDetalle()
        +CalcularDescuentoValor()
        +CalcularIVA()
        +ValidarPagos()
        +ValidarDetalle()
    }

    FacturaController --> IFacturacionUnitOfWork
    FacturacionUnitOfWork ..|> IFacturacionUnitOfWork
    FacturacionUnitOfWork --> IFacturacionRepository
    FacturacionRepository ..|> IFacturacionRepository
    FacturacionRepository --> DataContext
    FacturacionUnitOfWork --> FacturacionHelper
```

---

## ?? Estructura del Proyecto

```
SUPERMERCADO/
?
??? Supermercado.Backend/              # Proyecto principal de API
?   ??? Controllers/                   # Controladores de API REST
?   ?   ??? FacturaController.cs
?   ?   ??? ProductoController.cs
?   ?   ??? UsuarioController.cs
?   ?   ??? ...
?   ?
?   ??? Data/                          # Contexto de base de datos
?   ?   ??? DataContext.cs
?   ?   ??? SeedDb.cs                  # Datos iniciales
?   ?
?   ??? Helpers/                       # Clases auxiliares
?   ?   ??? FacturacionHelper.cs       # C�lculos y validaciones
?   ?
?   ??? Mapping/                       # Configuraci�n de AutoMapper
?   ?   ??? AutoMapperProfile.cs
?   ?
?   ??? Migrations/                    # Migraciones de EF Core
?   ?
?   ??? Repositories/                  # Capa de acceso a datos
?   ?   ??? Interfaces/
?   ?   ?   ??? IGenericRepository.cs
?   ?   ?   ??? IFacturacionRepository.cs
?   ?   ?   ??? ...
?   ?   ??? Implementations/
?   ?       ??? GenericRepository.cs
?   ?       ??? FacturacionRepository.cs
?   ?       ??? ...
?   ?
?   ??? UnitsOfWork/                   # L�gica de negocio
?   ?   ??? Interfaces/
?   ?   ?   ??? IGenericUnitOfWork.cs
?   ?   ?   ??? IFacturacionUnitOfWork.cs
?   ?   ?   ??? ...
?   ?   ??? Implementations/
?   ?       ??? GenericUnitOfWork.cs
?   ?       ??? FacturacionUnitOfWork.cs
?   ?       ??? ...
?   ?
?   ??? Program.cs                     # Punto de entrada
?   ??? appsettings.json               # Configuraci�n
?
??? Supermercado.Shared/               # Proyecto compartido
?   ??? DTOs/                          # Data Transfer Objects
?   ?   ??? FacturacionDTOs.cs
?   ?   ??? ProductoDTO.cs
?   ?   ??? UsuarioDTO.cs
?   ?   ??? ...
?   ?
?   ??? Entities/                      # Entidades del dominio
?   ?   ??? Factura.cs
?   ?   ??? Movimiento.cs
?   ?   ??? Detalle_Factura.cs
?   ?   ??? Producto.cs
?   ?   ??? Usuario.cs
?   ?   ??? ...
?   ?
?   ??? Responses/                     # Respuestas est�ndar
?       ??? ActionResponse.cs
?
??? Documentacion/                     # Documentaci�n del proyecto
?   ??? DOCUMENTACION_FACTURACION.md
?   ??? APLICACION_PRINCIPIOS_SOLID.md
?   ??? DOCUMENTACION_PATRONES_DISE�O.md
?
??? README.md                          # Este archivo
```

---

## ?? Principios de Desarrollo

### **Principios SOLID Aplicados**

#### **S - Single Responsibility Principle (Responsabilidad �nica)**
? Cada clase tiene una �nica responsabilidad bien definida:
- `FacturaController`: Maneja peticiones HTTP
- `FacturacionUnitOfWork`: Coordina l�gica de negocio
- `FacturacionRepository`: Accede a la base de datos
- `FacturacionHelper`: Provee utilidades de c�lculo

**Ejemplo:**
```csharp
// Controlador - Solo maneja HTTP
public class FacturaController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostAsync([FromBody] FacturaCompletaCreateDTO model)
    {
        var action = await _unitOfWork.CrearFacturaCompletaAsync(model);
        return Created($"/api/factura/{action.Result.FacturaId}", action.Result);
    }
}
```

#### **O - Open/Closed Principle (Abierto/Cerrado)**
? Clases abiertas para extensi�n, cerradas para modificaci�n:
- `GenericRepository<T>` es la base
- `FacturacionRepository` extiende sin modificar la base

**Ejemplo:**
```csharp
// Clase base - NO SE MODIFICA
public class GenericRepository<T> : IGenericRepository<T>
{
    public async Task<T> AddAsync(T entity) { /* ... */ }
}

// Extendemos para funcionalidad espec�fica
public class FacturacionRepository : IFacturacionRepository
{
    // M�todos espec�ficos de facturaci�n
    public async Task<ActionResponse<FacturaCompletaDTO>> CrearFacturaCompletaAsync(...)
    {
        // L�gica compleja de facturaci�n
    }
}
```

#### **L - Liskov Substitution Principle (Sustituci�n de Liskov)**
? Las interfaces permiten sustituir implementaciones:
```csharp
// Cualquier implementaci�n de IFacturacionRepository puede usarse
IFacturacionRepository repository = new FacturacionRepository(context);
```

#### **I - Interface Segregation Principle (Segregaci�n de Interfaces)**
? Interfaces espec�ficas por responsabilidad:
- `IFacturacionRepository` - Solo m�todos de facturaci�n
- `IProductoRepository` - Solo m�todos de productos
- `IUsuarioRepository` - Solo m�todos de usuarios

#### **D - Dependency Inversion Principle (Inversi�n de Dependencias)**
? Dependencias a trav�s de abstracciones (interfaces), no implementaciones:
```csharp
public class FacturacionUnitOfWork
{
    // Dependemos de la interfaz, no de la implementaci�n
    private readonly IFacturacionRepository _repository;
    
    public FacturacionUnitOfWork(IFacturacionRepository repository)
    {
        _repository = repository;
    }
}
```

---

## ?? Instalaci�n

### **Prerrequisitos**

- ? [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- ? [SQL Server 2022](https://www.microsoft.com/sql-server/sql-server-downloads) o superior
- ? [Visual Studio 2022](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- ? [Git](https://git-scm.com/)

### **Pasos de Instalaci�n**

1. **Clonar el repositorio**
```bash
git clone https://github.com/KevinMT98/SUPERMERCADO_POS.git
cd SUPERMERCADO_POS
```

2. **Configurar la cadena de conexi�n**

Edita `Supermercado.Backend/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=SupermercadoDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. **Restaurar paquetes NuGet**
```bash
cd Supermercado.Backend
dotnet restore
```

4. **Aplicar migraciones de base de datos**
```bash
dotnet ef database update
```

5. **Ejecutar el proyecto**
```bash
dotnet run
```

6. **Acceder a Swagger**

Abre tu navegador en: `https://localhost:7xxx/swagger`

### **Datos de Prueba**

El sistema incluye un `SeedDb` que carga datos iniciales:

**Usuarios:**
- **Admin:** `admin` / `Admin123!`
- **SuperAdmin:** `superadmin` / `Super123!`
- **Usuario:** `usuario1` / `User123!`

**Productos de ejemplo:**
- Coca Cola 2L
- Agua Cristal 600ml
- Leche Entera 1L
- Jab�n L�quido 500ml
- Arroz Blanco 1Kg

---

## ?? Documentaci�n

### **Documentaci�n Disponible**

?? **Documentaci�n Principal:**
- [Documentaci�n de Facturaci�n](DOCUMENTACION_FACTURACION.md) - Proceso completo de facturaci�n
- [Aplicaci�n de Principios SOLID](APLICACION_PRINCIPIOS_SOLID.md) - Implementaci�n de SOLID
- [Patrones de Dise�o](DOCUMENTACION_PATRONES_DISE�O.md) - Patrones utilizados

### **Ejemplo de Uso - Crear una Factura**

**Request:**
```json
POST /api/facturacion
Content-Type: application/json

{
  "terceroId": 1,
  "usuarioId": 1,
  "observaciones": "Venta mostrador",
  "detalles": [
    {
      "productoId": 1,
      "cantidad": 2,
      "precioUnitario": 15000.00,
      "descuentoPorcentaje": 10.0
    }
  ],
  "pagos": [
    {
      "metodoPagoId": 1,
      "monto": 27000.00,
      "referenciaPago": "Efectivo"
    }
  ]
}
```

**Response:**
```json
{
  "facturaId": 1,
  "numeroDocumento": "FV000001",
  "fecha": "2025-01-15T10:30:00",
  "nombreTercero": "Carlos G�mez",
  "totalBruto": 30000,
  "totalDescuentos": 3000,
  "totalImpuestos": 5130,
  "totalNeto": 32130,
  "detalles": [
    {
      "nombreProducto": "Coca Cola 2L",
      "cantidad": 2,
      "precioUnitario": 15000,
      "porcentajeIva": 19,
      "descuentoPorcentaje": 10,
      "descuentoValor": 3000,
      "subtotal": 32130
    }
  ],
  "pagos": [
    {
      "nombreMetodoPago": "Efectivo",
      "monto": 32130
    }
  ]
}
```

### **C�lculos de la Factura**

```
Subtotal Bruto    = 2 � $15,000 = $30,000
Descuento (10%)   = $30,000 � 10% = $3,000
Base Gravable     = $30,000 - $3,000 = $27,000
IVA (19%)         = $27,000 � 19% = $5,130
Total Neto        = $27,000 + $5,130 = $32,130
```

---

## ?? Autores

### **Equipo de Desarrollo**

| Nombre | Rol | GitHub |
|--------|-----|--------|
| **Angel Tovar** | Developer | - |
| **Kevin Monta�o** | Developer | [@KevinMT98](https://github.com/KevinMT98) |
| **Andres Felipe Yepes | Developer | - |
| **Cristian Camilo Gutierrez | Developer | - |


### **Informaci�n Acad�mica**

- **Universidad:** ITM (Instituto Tecnol�gico Metropolitano)
- **Asignatura:** Tecnolog�a en Desarrollo de Software
- **Fecha:** Octubre 2025
- **Versi�n:** 1.0

---

## ?? Licencia

Este proyecto est� bajo la Licencia MIT. Ver el archivo `LICENSE` para m�s detalles.

---

## ?? Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

---

## ?? Contacto

Para preguntas o sugerencias, contacta a:
- GitHub: [@KevinMT98](https://github.com/KevinMT98)
- Repositorio: [SUPERMERCADO_POS](https://github.com/KevinMT98/SUPERMERCADO_POS)

---

<div align="center">

**? Si este proyecto te fue �til, por favor dale una estrella ?**

Desarrollado con ?? usando .NET 9

</div>
