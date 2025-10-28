# ANÁLISIS DE PATRONES DE DISEÑO
## Sistema POS Supermercado

---

**Universidad:** ITM  
**Asignatura:** Tecnologia en desarrollo de software
**Estudiante:** Angel tovar - Kevin Montaño 
**Fecha:** Octubre 2025  
**Versión:** 1.0  

---

## TABLA DE CONTENIDOS

1. [Resumen Ejecutivo](#resumen-ejecutivo)
2. [Introducción](#introducción)
3. [Patrones de Diseño Implementados](#patrones-de-diseño-implementados)
4. [Arquitectura del Sistema](#arquitectura-del-sistema)
5. [Relación con Historias de Usuario](#relación-con-historias-de-usuario)
6. [Conclusiones](#conclusiones)

---

## RESUMEN EJECUTIVO

El presente documento analiza la implementación de patrones de diseño en el backend del Sistema POS Supermercado. Se identificaron **6 patrones principales** que garantizan una arquitectura limpia, mantenible y escalable:

1. **Repository Pattern** - Abstracción del acceso a datos
2. **Unit of Work Pattern** - Coordinación de transacciones
3. **Dependency Injection** - Inversión de control
4. **DTO Pattern** - Transferencia de datos entre capas
5. **Mapper Pattern** - Transformación automática de objetos
6. **Generic Repository Pattern** - Reutilización de código CRUD

---

## INTRODUCCIÓN

### Contexto del Proyecto

El Sistema POS Supermercado es una aplicación empresarial para gestionar operaciones de punto de venta e inventario.

**Stack Tecnológico:**
- ASP.NET Core 9.0
- Entity Framework Core
- SQL Server
- AutoMapper
- JWT Authentication

### Objetivos del especificos

1. Identificar patrones de diseño implementados
2. Documentar fragmentos de código representativos
3. Explicar relación con historias de usuario
4. Demostrar cumplimiento de reglas de negocio

---

## PATRONES DE DISEÑO IMPLEMENTADOS

### 1. Repository Pattern

**Descripción:** Abstrae la lógica de acceso a datos proporcionando una interfaz de colección.

**Ubicación:**
- `Repositories\Interfaces\IProductoRepository.cs`
- `Repositories\Implementations\ProductoRepository.cs`

**Código Representativo:**

```csharp
public interface IProductoRepository : IGenericRepository<Producto>
{
    Task<bool> ExistsByCodigoProductoAsync(string codigoProducto, int? excludeId = null);
    Task<ActionResponse<IEnumerable<Producto>>> GetProductosConStockBajoAsync();
}

public class ProductoRepository : GenericRepository<Producto>, IProductoRepository
{
    public async Task<ActionResponse<IEnumerable<Producto>>> GetProductosConStockBajoAsync()
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
}
```

**Aplicación HU-009:** El método `GetProductosConStockBajoAsync()` implementa la detección automática de productos con stock bajo.

---

### 2. Unit of Work Pattern

**Descripción:** Coordina el trabajo de múltiples repositorios manteniendo transacciones.

**Ubicación:**
- `UnitsOfWork\Interfaces\IProductoUnitOfWork.cs`
- `UnitsOfWork\Implementations\ProductoUnitOfWork.cs`

**Código Representativo:**

```csharp
public interface IProductoUnitOfWork : IGenericUnitOfWork<Producto>
{
    Task<bool> ExistsByCodigoProductoAsync(string codigoProducto, int? excludeId = null);
    Task<bool> ExistsByCodigoBarrasAsync(string codigoBarras, int? excludeId = null);
    Task<ActionResponse<IEnumerable<Producto>>> GetProductosConStockBajoAsync();
}

public class ProductoUnitOfWork : GenericUnitOfWork<Producto>, IProductoUnitOfWork
{
    private readonly IProductoRepository _productoRepository;

    public async Task<bool> ExistsByCodigoProductoAsync(string codigoProducto, int? excludeId = null)
        => await _productoRepository.ExistsByCodigoProductoAsync(codigoProducto, excludeId);
}
```

**Aplicación HU-CRUD:** Valida unicidad de códigos antes de crear/actualizar productos.

---

### 3. Dependency Injection

**Descripción:** Implementa inversión de control para desacoplar componentes.

**Ubicación:** `Program.cs`

**Código Representativo:**

```csharp
// Configuración de Iyeccion de dependencias
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoUnitOfWork, ProductoUnitOfWork>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Uso en controlador
public class ProductoController : ControllerBase
{
    private readonly IProductoUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ProductoController(IProductoUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
}
```
---

### 4. DTO Pattern

**Descripción:** Objetos para transferir datos entre capas, separando modelo de dominio.

**Ubicación:** `Supermercado.Shared\DTOs\ProductoDTO.cs`

**Código Representativo:**

```csharp
public class ProductoCreateDto
{
    [Required(ErrorMessage = "El código del producto es obligatorio")]
    [MaxLength(50)]
    public string CodigoProducto { get; set; } = null!;

    [Required(ErrorMessage = "El stock mínimo es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0")]
    public int StockMinimo { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int StockMaximo { get; set; }
}
```

**Aplicación HU-009:** 
- Campo `StockMinimo` obligatorio y validado
- `Range(0, int.MaxValue)` garantiza valores no negativos
- Cumple regla: "El valor mínimo no puede ser negativo"

---

### 5. Mapper Pattern (AutoMapper)

**Descripción:** Automatiza transformación entre entidades y DTOs.

**Ubicación:** `Mapping\AutoMapperProfile.cs`

**Código Representativo:**

```csharp
public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Producto, ProductoDto>()
            .ForMember(dest => dest.ProductoId, opt => opt.MapFrom(src => src.producto_id))
            .ForMember(dest => dest.StockMinimo, opt => opt.MapFrom(src => src.stock_minimo));

        CreateMap<ProductoCreateDto, Producto>()
            .ForMember(dest => dest.stock_minimo, opt => opt.MapFrom(src => src.StockMinimo))
            .ForMember(dest => dest.producto_id, opt => opt.Ignore());
    }
}

// Uso en controlador
var entity = _mapper.Map<Producto>(model);
var dto = _mapper.Map<ProductoDto>(action.Result);
```

**Beneficio:** Elimina código repetitivo de mapeo manual, evita presentar datos sensibles de la base de datos.

---

### 6. Generic Repository Pattern

**Descripción:** Implementación genérica de operaciones CRUD para cualquier entidad.

**Ubicación:** `Repositories\Implementations\GenericRepository.cs`

**Código Representativo:**

```csharp
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
            return new ActionResponse<T> { Message = "El registro no existe." };
        
        _entity.Remove(row);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T> { WasSuccess = true };
        }
        catch 
        {
            return new ActionResponse<T>
            {
                Message = "No se puede eliminar el registro porque tiene relaciones con otros registros."
            };
        }
    }
}
```

**Aplicación HU-CRUD:** Implementa regla "No se debe permitir la eliminación de productos que tengan movimientos asociados".

---

## ARQUITECTURA DEL SISTEMA

### Arquitectura en Capas

```
┌─────────────────────────────────────────┐
│      CAPA PRESENTACIÓN                  │
│  ProductoController (API REST)          │
│  - Validaciones de negocio              │
│  - Transformación DTO ↔ Entidad         │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│      CAPA LÓGICA NEGOCIO                │
│  ProductoUnitOfWork                     │
│  - ExistsByCodigoProductoAsync()        │
│  - GetProductosConStockBajoAsync()      │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│      CAPA ACCESO DATOS                  │
│  ProductoRepository                     │
│  - Operaciones CRUD                     │
│  - Consultas específicas                │
└─────────────────────────────────────────┘
              ↓
┌─────────────────────────────────────────┐
│      CAPA PERSISTENCIA                  │
│  Entity Framework Core + SQL Server     │
└─────────────────────────────────────────┘
```

### Flujo: Crear Producto

```csharp
[HttpPost]
public async Task<IActionResult> PostAsync([FromBody] ProductoCreateDto model)
{
    // 1. Validar código único
    if (await _unitOfWork.ExistsByCodigoProductoAsync(model.CodigoProducto))
        return BadRequest("El código de producto ya existe.");
    
    // 2. Validar código de barras único
    if (await _unitOfWork.ExistsByCodigoBarrasAsync(model.CodigoBarras))
        return BadRequest("El código de barras ya existe.");
    
    // 3. Mapear DTO → Entidad
    var entity = _mapper.Map<Producto>(model);
    
    // 4. Guardar en base de datos
    var action = await _unitOfWork.AddAsync(entity);
    
    // 5. Mapear Entidad → DTO
    var dto = _mapper.Map<ProductoDto>(action.Result);
    return Created($"/api/producto/{dto.ProductoId}", dto);
}
```

**Patrones aplicados:** DTO, Mapper, Unit of Work, Repository, Dependency Injection

---

## RELACIÓN CON HISTORIAS DE USUARIO

### HU-CRUD-Productos

| Requisito | Implementación | Ubicación |
|-----------|----------------|-----------|
| Código único | `ExistsByCodigoProductoAsync()` | ProductoController.cs:69-70 |
| Código barras único | `ExistsByCodigoBarrasAsync()` | ProductoController.cs:71-72 |
| No eliminar con relaciones | Catch en `DeleteAsync()` | GenericRepository.cs:62-68 |
| Crear producto | `PostAsync()` | ProductoController.cs:66-82 |
| Consultar productos | `GetAsync()` | ProductoController.cs:34-40 |
| Actualizar producto | `PutAsync()` | ProductoController.cs:93-114 |
| Eliminar producto | `DeleteAsync()` | ProductoController.cs:124-129 |

**Reglas de Negocio Implementadas:**

1. **Código único de producto:**
```csharp
if (await _unitOfWork.ExistsByCodigoProductoAsync(model.CodigoProducto))
    return BadRequest("El código de producto ya existe.");
```

2. **No eliminar productos con relaciones:**
```csharp
catch 
{
    return new ActionResponse<T>
    {
        Message = "No se puede eliminar el registro porque tiene relaciones con otros registros."
    };
}
```

---

### HU-009 - Mínimos de Inventario

| Requisito | Implementación | Ubicación |
|-----------|----------------|-----------|
| Campo stock_minimo | Propiedad en entidad | Producto.cs:39-40 |
| Validación no negativo | `[Range(0, int.MaxValue)]` | ProductoDTO.cs:58-60 |
| Campo obligatorio | `[Required]` | ProductoDTO.cs:58 |
| Consulta stock bajo | `GetProductosConStockBajoAsync()` | ProductoRepository.cs:45-69 |
| Lógica de alerta | `WHERE stock_actual < stock_minimo` | ProductoRepository.cs:50 |

**Reglas de Negocio Implementadas:**

1. **Campo stock mínimo configurable:**
```csharp
[Required(ErrorMessage = "El stock mínimo es obligatorio")]
[Range(0, int.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a 0")]
public int StockMinimo { get; set; }
```

2. **Detección automática de stock bajo:**
```csharp
public async Task<ActionResponse<IEnumerable<Producto>>> GetProductosConStockBajoAsync()
{
    var productos = await _context.Productos
        .Where(p => p.stock_actual < p.stock_minimo && p.activo)
        .Include(p => p.Categoria)
        .Include(p => p.TarifaIVA)
        .ToListAsync();
    // ...
}
```

3. **Validación de valores:**
- ✅ Stock mínimo no puede ser negativo (`Range(0, int.MaxValue)`)
- ✅ Campo configurable por administrador (presente en CreateDto y UpdateDto)
- ✅ Sistema genera alertas automáticas (método `GetProductosConStockBajoAsync()`)

---

## BENEFICIOS DE LA ARQUITECTURA

### Mantenibilidad
- ✅ Separación clara de responsabilidades (SRP)
- ✅ Código reutilizable (Generic Repository)
- ✅ Fácil localización y corrección de bugs
- ✅ Documentación mediante interfaces

### Escalabilidad
- ✅ Agregar nuevas entidades es trivial
- ✅ Patrones consistentes en toda la aplicación
- ✅ Fácil agregar nuevas reglas de negocio
- ✅ Arquitectura preparada para microservicios

### Testabilidad
- ✅ Interfaces permiten crear mocks fácilmente
- ✅ Lógica de negocio aislada en Unit of Work
- ✅ DTOs facilitan pruebas de validación
- ✅ Dependency Injection facilita testing unitario

### Calidad del Código
- ✅ Código limpio y bien documentado
- ✅ Validaciones en múltiples capas
- ✅ Manejo robusto de errores
- ✅ Cumplimiento de principios SOLID

---

## CONCLUSIONES

### Resumen de Implementación

El backend del Sistema POS Supermercado implementa una **arquitectura profesional y robusta** basada en patrones de diseño reconocidos:

1. **Repository + Unit of Work**: Abstracción completa del acceso a datos
2. **Dependency Injection**: Inversión de control para mejor testabilidad
3. **DTO Pattern**: Separación entre modelo de dominio y API
4. **Mapper Pattern**: Transformaciones automáticas y consistentes
5. **Generic Repository**: Reutilización de código para operaciones comunes

### Cumplimiento de Requisitos

**HU-CRUD-Productos:** ✅ Completamente implementada
- Todas las operaciones CRUD funcionales
- Validaciones de unicidad implementadas
- Protección contra eliminación de registros con relaciones

**HU-009 - Mínimos de Inventario:** ✅ Completamente implementada
- Campo stock_minimo configurable
- Validaciones de valores no negativos
- Sistema de alertas automáticas implementado

### Preparación para el MVP

- ✅ Todas las funcionalidades requeridas están implementadas
- ✅ Código preparado para crecimiento futuro
- ✅ Arquitectura escalable y mantenible
- ✅ Cumplimiento de mejores prácticas de desarrollo

---

## REFERENCIAS

- **Fowler, M.** (2002). *Patterns of Enterprise Application Architecture*. Addison-Wesley.
- **Microsoft Docs** (2024). *ASP.NET Core Dependency Injection*. https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection
- **AutoMapper Documentation** (2024). https://docs.automapper.org
- **Entity Framework Core Documentation** (2024). https://docs.microsoft.com/ef/core
- **Repository Pattern** - Martin Fowler: https://martinfowler.com/eaaCatalog/repository.html
- **Unit of Work Pattern** - Martin Fowler: https://martinfowler.com/eaaCatalog/unitOfWork.html
