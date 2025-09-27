# SUPERMERCADO_POS

# Supermercado POS

Este proyecto es un sistema de punto de venta (POS) para un supermercado, desarrollado con .NET 9 y Blazor, siguiendo principios de Clean Architecture para lograr un código desacoplado, mantenible y escalable.

## Estructura del Proyecto

- **Supermercado.Shared**  
  Contiene las entidades y modelos compartidos entre el backend y el frontend.

- **Supermercado.Backend**  
  Proyecto ASP.NET Core Web API. Expone endpoints RESTful para la gestión de roles, categorías de productos y otras entidades.  
  Implementa patrones Repository y Unit of Work para el acceso a datos y lógica de negocio.

## Principales Características

- Arquitectura limpia (Clean Architecture) y desacoplada.
- CRUD genérico para entidades como `Rol` y `Categoria_Producto`.
- Uso de patrones Repository y Unit of Work.
- Inyección de dependencias para servicios y repositorios.
- Base de datos relacional (SQL Server) gestionada con Entity Framework Core.
- Población automática de datos iniciales (seeding) para roles y categorías.
- Documentación automática de la API con Swagger.


## Cómo ejecutar el proyecto

1. **Clona el repositorio**
2. **Configura la cadena de conexión** en `Supermercado.Backend/appsettings.json`.
3. **Restaura los paquetes NuGet** y ejecuta las migraciones si es necesario.
4. **Ejecuta el backend** (`Supermercado.Backend`) para exponer la API.
5. Accede a la documentación de la API en `/swagger` (por ejemplo, `http://localhost:5276/swagger`).

## Estructura de carpetas relevante

- `Supermercado.Shared/Entities` — Entidades del dominio (por ejemplo, `Rol`, `Categoria_Producto`)
- `Supermercado.Backend/Controllers` — Controladores de la API
- `Supermercado.Backend/Repositories` — Repositorios genéricos y específicos
- `Supermercado.Backend/UnitsOfWork` — Implementaciones de Unit of Work
- `Supermercado.Backend/Data` — Contexto de datos y seeding
- `Supermercado.Frontend/Components` — Componentes de Blazor

## Tecnologías utilizadas

- .NET 9
- ASP.NET Core Web API
- Blazor
- Entity Framework Core
- SQL Server
- Swagger

## Estado actual

- CRUD funcional para roles y categorías de productos.
- Arquitectura desacoplada y lista para escalar a nuevas entidades y funcionalidades.
- Base para agregar autenticación, autorización, ventas, inventario y más módulos.

---

> Proyecto desarrollado como ejemplo de arquitectura limpia y buenas prácticas en .NET y Blazor.