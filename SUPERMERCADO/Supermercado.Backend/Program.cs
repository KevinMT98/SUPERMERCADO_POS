using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Supermercado.Backend.Data;
using Supermercado.Backend.Mapping;
using Supermercado.Backend.Repositories.Implementations;
using Supermercado.Backend.Repositories.Interfaces;
using Supermercado.Backend.UnitsOfWork.Implementations;
using Supermercado.Backend.UnitsOfWork.Interfaces;
using System.Text;

// Crear el builder para configurar la aplicación web
var builder = WebApplication.CreateBuilder(args);

// ===== CONFIGURACIÓN DE SERVICIOS =====

// Registrar los controladores de API para manejar las peticiones HTTP
builder.Services.AddControllers();

// Habilitar la exploración de endpoints para Swagger
builder.Services.AddEndpointsApiExplorer();

// Configurar Swagger para documentación automática de la API
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Supermercado API",
        Version = "v1",
        Description = "API para gestión de supermercado"
    });



// Configurar JWT en Swagger
options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    Description = "JWT Authorization header. Puede enviar: \"Bearer {token}\" o solo \"{token}\"",
    Name = "Authorization",
    In = ParameterLocation.Header,
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
});

options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configurar Entity Framework con SQL Server
// Utiliza la cadena de conexión "DefaultConnection" del archivo appsettings.json
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar autenticación JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SuperSecretKey_ChangeInProduction_MinLength32Characters!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "SupermercadoAPI";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "SupermercadoClient";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader))
            {
                if (authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    context.Token = authHeader.Substring("Bearer ".Length).Trim();
                }
                else
                {
                    context.Token = authHeader.Trim();
                }
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();

// Configurar CORS para permitir consumo desde diferentes clientes
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("X-Total-Count");
    });
});

// Registrar el servicio para poblar datos iniciales en la base de datos
builder.Services.AddTransient<SeedDb>();

// ===== REGISTRAR AUTOMAPPER =====
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// ===== INYECCIÓN DE DEPENDENCIAS =====
// Implementación del patrón Repository y Unit of Work para separar la lógica de acceso a datos

// Repositorios y Units of Work genéricos (para entidades simples sin lógica adicional)
builder.Services.AddScoped(typeof(IGenericUnitOfWork<>), typeof(GenericUnitOfWork<>));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Repositorios y Units of Work específicos para Producto
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoUnitOfWork, ProductoUnitOfWork>();

// Repositorios y Units of Work específicos para Tarifa_IVA
builder.Services.AddScoped<ITarifaIvaRepository, TarifaIvaRepository>();
builder.Services.AddScoped<ITarifaIvaUnitOfWork, TarifaIvaUnitOfWork>();

// Repositorios y Units of Work específicos para Categoria_Producto
builder.Services.AddScoped<ICategoriaProductoRepository, CategoriaProductoRepository>();
builder.Services.AddScoped<ICategoriaProductoUnitOfWork, CategoriaProductoUnitOfWork>();

// Repositorios y Units of Work específicos para Rol
builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<IRolUnitOfWork, RolUnitOfWork>();

// Repositorios y Units of Work específicos para Autenticación
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();

// Repositorios y Units of Work específicos para Maestro de Usuarios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioUnitOfWork, UsuarioUnitOfWork>();

// Construir la aplicación con todas las configuraciones anteriores
var app = builder.Build();

// Ejecutar el proceso de población de datos iniciales al iniciar la aplicación y creación de base de datos
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
    using var scope = scopeFactory!.CreateScope();
    
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

// ===== CONFIGURAR MIDDLEWARE PIPELINE =====
// Configurar CORS para permitir peticiones desde el frontend
app.UseCors("AllowAll");

// Agregar middleware de autenticación y autorización (ORDEN IMPORTANTE)
app.UseAuthentication(); // Debe ir antes de UseAuthorization
app.UseAuthorization();

// Mapear los controladores para que respondan a las rutas configuradas
app.MapControllers();

// Iniciar la aplicación y comenzar a escuchar peticiones
app.Run();
