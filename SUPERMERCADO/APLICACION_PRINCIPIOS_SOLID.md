**Universidad:** ITM  
**Asignatura:** Tecnologia en desarrollo de software
**Estudiante:** Angel tovar - Kevin Montaño 
**Fecha:** Octubre 2025  
**Versión:** 1.0  

# Aplicación de Principios SOLID - Sistema POS Supermercado

## Historia de Usuario: CRUD - Productos

**Como** Administrador / Encargado de almacén  
**Quiero** Crear, consultar, actualizar y eliminar productos en el sistema  
**Para** Mantener actualizada la información de los artículos disponibles para la venta y el control del inventario

### Reglas de Negocio
- Cada producto debe tener un código único, nombre, categoría, precio de venta, costo y stock inicial.
- No se debe permitir la eliminación de productos que tengan movimientos asociados (ventas o compras).
- Las actualizaciones deben quedar registradas en el historial de movimientos.

---

## Principios SOLID Aplicados

### 1. S – Responsabilidad Única (Single Responsibility)

**¿Qué significa?** Cada clase debe tener una sola tarea o responsabilidad.

#### ¿Cómo lo aplicamos?

Separamos las responsabilidades en diferentes clases:

- **`ProductoController`**: Solo maneja las peticiones HTTP (recibe solicitudes y devuelve respuestas)
- **`ProductoUnitOfWork`**: Coordina la lógica de negocio (validaciones y reglas)
- **`ProductoRepository`**: Solo accede a la base de datos
- **`AutoMapperProfile`**: Solo convierte objetos entre diferentes formatos

**Ejemplo práctico:**

```csharp
// ProductoController - Solo maneja HTTP
public class ProductoController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var action = await _unitOfWork.GetAsync();
        return Ok(dtos);
    }
}

// ProductoRepository - Solo accede a la base de datos
public class ProductoRepository : GenericRepository<Producto>
{
    public async Task<bool> ExistsByCodigoProductoAsync(string codigoProducto)
    {
        return await _context.Productos.AnyAsync(p => p.codigo_producto == codigoProducto);
    }
}
```

**Beneficio:** Si necesitamos cambiar cómo se guardan los datos, solo modificamos el repositorio. Si cambiamos la API, solo tocamos el controlador.

---

### 2. O – Abierto/Cerrado (Open/Closed)

**¿Qué significa?** Podemos agregar nuevas funcionalidades sin modificar el código existente.

#### ¿Cómo lo aplicamos?

Creamos clases genéricas que sirven de base y las extendemos para casos específicos:

**Ejemplo práctico:**

```csharp
// Clase base genérica - NO SE MODIFICA
public class GenericRepository<T>
{
    public async Task<T> GetAsync(int id) { /* ... */ }
    public async Task<T> AddAsync(T entity) { /* ... */ }
    // Métodos comunes para todas las entidades
}

// Extendemos para Producto - AGREGAMOS funcionalidad
public class ProductoRepository : GenericRepository<Producto>
{
    // Método específico para productos
    public async Task<bool> ExistsByCodigoProductoAsync(string codigo)
    {
        return await _context.Productos.AnyAsync(p => p.codigo_producto == codigo);
    }
    
    // Otro método específico
    public async Task<List<Producto>> GetProductosConStockBajoAsync()
    {
        return await _context.Productos.Where(p => p.stock_actual < p.stock_minimo).ToListAsync();
    }
}
```

**Beneficio:** Podemos crear `CategoriaRepository`, `UsuarioRepository`, etc., reutilizando `GenericRepository` sin modificarlo.

---

### 3. L – Sustitución de Liskov (Liskov Substitution)

**¿Qué significa?** Las clases hijas deben poder usarse en lugar de las clases padre sin problemas.

#### ¿Cómo lo aplicamos?

`ProductoRepository` hereda de `GenericRepository` y puede usarse en cualquier lugar donde se espere un repositorio genérico:

**Ejemplo práctico:**

```csharp
// Ambas opciones funcionan correctamente
IGenericRepository<Producto> repositorio;

// Opción 1: Usar la clase base
repositorio = new GenericRepository<Producto>(context);

// Opción 2: Usar la clase específica (funciona igual + métodos extra)
repositorio = new ProductoRepository(context);

// Los métodos base funcionan en ambos casos
var producto = await repositorio.GetAsync(1);
var productos = await repositorio.GetAsync();
```

**Beneficio:** Podemos intercambiar implementaciones sin romper el código. El sistema es flexible y mantenible.

---

### 4. I – Segregación de Interfaces (Interface Segregation)

**¿Qué significa?** Las clases no deben estar obligadas a implementar métodos que no usan.

#### ¿Cómo lo aplicamos?

Creamos interfaces específicas para cada necesidad en lugar de una interfaz grande con todo:

**Ejemplo práctico:**

```csharp
// ✅ BIEN - Interfaz base con lo común
public interface IGenericUnitOfWork<T>
{
    Task<T> GetAsync(int id);
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(int id);
}

// ✅ BIEN - Interfaz específica solo para Producto
public interface IProductoUnitOfWork : IGenericUnitOfWork<Producto>
{
    Task<bool> ExistsByCodigoProductoAsync(string codigo);
    Task<bool> ExistsByCodigoBarrasAsync(string barras);
    Task<List<Producto>> GetProductosConStockBajoAsync();
}
```

**También aplicamos esto en los DTOs:**

```csharp
// DTO para consultar (GET) - solo lectura
public class ProductoDto
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; }
}

// DTO para crear (POST) - sin ID
public class ProductoCreateDto
{
    public string Nombre { get; set; }
    public decimal Precio { get; set; }
}

// DTO para actualizar (PUT) - con ID
public class ProductoUpdateDto
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; }
}
```

**Beneficio:** Cada parte del sistema usa solo lo que necesita. No hay métodos innecesarios.

---

### 5. D – Inversión de Dependencias (Dependency Inversion)

**¿Qué significa?** Las clases no deben depender de implementaciones concretas, sino de interfaces (abstracciones).

#### ¿Cómo lo aplicamos?

Usamos interfaces en lugar de clases concretas, y las inyectamos mediante Dependency Injection:

**Ejemplo práctico:**

```csharp
// ✅ BIEN - Depende de la interfaz
public class ProductoController : ControllerBase
{
    private readonly IProductoUnitOfWork _unitOfWork;  // Interfaz, no clase concreta
    
    public ProductoController(IProductoUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
}

// Configuración en Program.cs
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoUnitOfWork, ProductoUnitOfWork>();
```

**Flujo de dependencias:**

```
ProductoController → IProductoUnitOfWork → IProductoRepository → Base de Datos
```

**Beneficio:** Podemos cambiar la implementación (por ejemplo, cambiar de SQL Server a MongoDB) sin modificar el controlador. También facilita las pruebas unitarias usando mocks.

---

## Beneficios Obtenidos

- **Mantenibilidad:** Código organizado y fácil de modificar
- **Escalabilidad:** Fácil agregar nuevas entidades reutilizando código
- **Testabilidad:** Se pueden hacer pruebas unitarias sin base de datos
- **Flexibilidad:** Cambiar tecnologías sin afectar todo el sistema
- **Reutilización:** Clases genéricas usadas por múltiples entidades

---

## Cumplimiento de Reglas de Negocio

### Regla 1: Código único de producto

```csharp
if (await _unitOfWork.ExistsByCodigoProductoAsync(model.CodigoProducto))
    return BadRequest("El código de producto ya existe.");
```

**Principios SOLID aplicados:** S (validación separada), D (usa interfaz)

### Regla 2: No eliminar productos con movimientos

```csharp
catch 
{
    return new ActionResponse<T>
    {
        Message = "No se puede eliminar el registro porque tiene relaciones."
    };
}
```

**Principios SOLID aplicados:** O (se puede extender), L (mantiene contrato)

### Regla 3: Historial de actualizaciones

Se puede implementar sobrescribiendo el método `UpdateAsync` en `ProductoRepository` para registrar cambios sin modificar la clase base.

**Principios SOLID aplicados:** O (extensión), S (responsabilidad clara)

---

## Conclusión

El proyecto **Supermercado** implementa correctamente los **5 principios SOLID** en la historia de usuario **CRUD - Productos**, logrando una arquitectura limpia, mantenible y escalable. La separación en capas (Controller → UnitOfWork → Repository) con dependencia de abstracciones permite cumplir con las reglas de negocio de manera flexible y testeable.
