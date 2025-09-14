using Microsoft.EntityFrameworkCore;
using Supermercado.Backend.Data;
using Supermercado.Backend.Repositories.Implementations;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Implementations;
using Supermercado.Backend.UnitsOfWork.Interfaces;

// Crear el builder para configurar la aplicaci�n web
var builder = WebApplication.CreateBuilder(args);

// ===== CONFIGURACI�N DE SERVICIOS =====

// Registrar los controladores de API para manejar las peticiones HTTP
builder.Services.AddControllers();

// Habilitar la exploraci�n de endpoints para Swagger
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger para documentaci�n autom�tica de la API
builder.Services.AddSwaggerGen();

// Configurar Entity Framework con SQL Server
// Utiliza la cadena de conexi�n "DefaultConnection" del archivo appsettings.json
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar el servicio para poblar datos iniciales en la base de datos
builder.Services.AddTransient<SeedDb>();

// ===== INYECCI�N DE DEPENDENCIAS =====
// Implementaci�n del patr�n Repository y Unit of Work para separar la l�gica de acceso a datos

// Registrar la unidad de trabajo gen�rica - coordina transacciones y operaciones sobre m�ltiples repositorios
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));

// Registrar el repositorio gen�rico - permite operaciones CRUD reutilizables para cualquier entidad
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Construir la aplicaci�n con todas las configuraciones anteriores
var app = builder.Build();

// Ejecutar el proceso de poblaci�n de datos iniciales al iniciar la aplicaci�n y crecion de base de datos
SeedData(app);

/// <summary>
/// M�todo para poblar la base de datos con datos iniciales
/// Se ejecuta al iniciar la aplicaci�n para asegurar que existan datos de prueba
/// </summary>
/// <param name="app">La aplicaci�n web construida</param>
void SeedData(WebApplication app)
{
    // Obtener el factory para crear scopes de servicios
    var scopeFactory = app.Services.GetService<IServiceScopeFactory>();

    // Crear un scope para resolver dependencias de forma controlada
    using var scope = scopeFactory.CreateScope();
    
    // Obtener el servicio SeedDb para poblar datos
    var service = scope.ServiceProvider.GetRequiredService<SeedDb>();
    
    // Ejecutar la poblaci�n de datos de forma s�ncrona
    service!.SeedAsync().Wait();
}

// ===== CONFIGURACI�N DEL PIPELINE DE MIDDLEWARE =====

// Solo habilitar Swagger en ambiente de desarrollo por seguridad
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger para generar la documentaci�n JSON de la API
    app.UseSwagger();
    
    // Habilitar Swagger UI para la interfaz web interactiva de la API
    app.UseSwaggerUI();
}

// Comentados por ahora - se pueden habilitar seg�n las necesidades de seguridad
//app.UseHttpsRedirection(); // Forzar redirecci�n HTTPS
//app.UseAuthorization();    // Habilitar autorizaci�n

// Mapear los controladores para que respondan a las rutas configuradas
app.MapControllers();

// Iniciar la aplicaci�n y comenzar a escuchar peticiones
app.Run();
