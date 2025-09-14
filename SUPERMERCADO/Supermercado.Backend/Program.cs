using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Implementations;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Implementations;
using Supermercado.Backend.UnitsOfWork.Interfaces;

// Crear el builder para configurar la aplicación web
var builder = WebApplication.CreateBuilder(args);

// ===== CONFIGURACIÓN DE SERVICIOS =====

// Registrar los controladores de API para manejar las peticiones HTTP
builder.Services.AddControllers();

// Habilitar la exploración de endpoints para Swagger
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger para documentación automática de la API
builder.Services.AddSwaggerGen();

// Configurar Entity Framework con SQL Server
// Utiliza la cadena de conexión "DefaultConnection" del archivo appsettings.json
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el servicio para poblar datos iniciales en la base de datos
builder.Services.AddTransient<SeedDb>();

// ===== INYECCIÓN DE DEPENDENCIAS =====
// Implementación del patrón Repository y Unit of Work para separar la lógica de acceso a datos

// Registrar la unidad de trabajo genérica - coordina transacciones y operaciones sobre múltiples repositorios
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));

// Registrar el repositorio genérico - permite operaciones CRUD reutilizables para cualquier entidad
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Construir la aplicación con todas las configuraciones anteriores
var app = builder.Build();

// Ejecutar el proceso de población de datos iniciales al iniciar la aplicación y crecion de base de datos
SeedData(app);

/// <summary>
/// Método para poblar la base de datos con datos iniciales
/// Se ejecuta al iniciar la aplicación para asegurar que existan datos de prueba
/// </summary>
/// <param name="app">La aplicación web construida</param>
void SeedData(WebApplication app)
{
    // Obtener el factory para crear scopes de servicios
    var scopeFactory = app.Services.GetService<IServiceScopeFactory>();

    // Crear un scope para resolver dependencias de forma controlada
    using var scope = scopeFactory.CreateScope();
    
    // Obtener el servicio SeedDb para poblar datos
    var service = scope.ServiceProvider.GetRequiredService<SeedDb>();
    
    // Ejecutar la población de datos de forma síncrona
    service!.SeedAsync().Wait();
}

// ===== CONFIGURACIÓN DEL PIPELINE DE MIDDLEWARE =====

// Solo habilitar Swagger en ambiente de desarrollo por seguridad
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger para generar la documentación JSON de la API
    app.UseSwagger();
    
    // Habilitar Swagger UI para la interfaz web interactiva de la API
    app.UseSwaggerUI();
}

// Comentados por ahora - se pueden habilitar según las necesidades de seguridad
//app.UseHttpsRedirection(); // Forzar redirección HTTPS
//app.UseAuthorization();    // Habilitar autorización

// Mapear los controladores para que respondan a las rutas configuradas
app.MapControllers();

// Iniciar la aplicación y comenzar a escuchar peticiones
app.Run();
