# Resolución del Error: IAuthUnitOfWork no registrado

## ? Error Original
```
System.InvalidOperationException: Unable to resolve service for type 'Supermercado.Backend.UnitsOfWork.Interfaces.IAuthUnitOfWork' while attempting to activate 'Supermercado.Backend.Controllers.AuthController'.
```

## ?? Cambios Realizados para Resolver el Error

### 1. **Registro de Servicios de Autenticación en Program.cs**
```csharp
// Repositorios y Units of Work específicos para Autenticación/Usuario
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();
```

### 2. **Configuración JWT en appsettings.json**
```json
{
  "Jwt": {
    "Key": "SuperSecretKey_ChangeInProduction_MinLength32Characters!",
    "Issuer": "SupermercadoAPI",
    "Audience": "SupermercadoClient"
  }
}
```

### 3. **Middleware de Autenticación en el Pipeline**
```csharp
// ORDEN IMPORTANTE: Authentication debe ir antes que Authorization
app.UseAuthentication();
app.UseAuthorization();
```

### 4. **Configuración Completa de JWT en Program.cs**
Ya estaba configurada correctamente la autenticación JWT con:
- `TokenValidationParameters` apropiados
- Configuración de Bearer tokens
- Soporte para Swagger con JWT

## ? Servicios Ahora Registrados

| Servicio | Implementación | Descripción |
|----------|----------------|-------------|
| `IUserRepository` | `UserRepository` | Acceso a datos de usuarios |
| `IAuthUnitOfWork` | `AuthUnitOfWork` | Lógica de autenticación |
| JWT Authentication | `JwtBearerDefaults` | Validación de tokens |
| Authorization | ASP.NET Core | Autorización por roles |

## ?? Endpoints de Autenticación Disponibles

### POST /auth/login
```json
{
  "email": "admin@supermercado.com",
  "password": "Admin123!"
}
```

**Respuesta exitosa:**
```json
{
  "accesToken": "eyJhbGciOiJIUzI1NiIs...",
  "expireIn": 3600,
  "email": "admin@supermercado.com",
  "role": "Admin"
}
```

### Usuarios de Prueba Disponibles

| Usuario | Email | Contraseña | Rol | Estado |
|---------|-------|------------|-----|--------|
| admin | admin@supermercado.com | Admin123! | Admin | Activo |
| superadmin | superadmin@supermercado.com | Super123! | Admin | Activo |
| usuario1 | juan.perez@supermercado.com | User123! | User | Activo |
| usuario2 | maria.garcia@supermercado.com | User123! | User | Activo |

## ?? Uso del Token JWT

### En Swagger:
1. Haz clic en **"Authorize"**
2. Ingresa: `Bearer {tu_token_aqui}`
3. O simplemente: `{tu_token_aqui}`

### En Headers HTTP:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIs...
```

## ?? Pruebas Recomendadas

### 1. **Test de Login**
```bash
curl -X POST "http://localhost:5276/auth/login" \
-H "Content-Type: application/json" \
-d '{"email":"admin@supermercado.com","password":"Admin123!"}'
```

### 2. **Test de Endpoint Protegido**
```bash
curl -X GET "http://localhost:5276/api/categoria_producto" \
-H "Authorization: Bearer {token_obtenido}"
```

### 3. **Test de Autorización por Rol**
- Los endpoints con `[Authorize(Roles = "Admin")]` solo funcionarán con usuarios Admin
- Los endpoints con `[Authorize]` funcionarán con cualquier usuario autenticado

## ?? Estado Actual

? **Servicios registrados correctamente**  
? **JWT configurado y funcional**  
? **Middleware de autenticación activo**  
? **Endpoints protegidos operativos**  
? **Datos semilla con usuarios de prueba**  
? **Swagger con soporte JWT**  

## ?? Próximos Pasos

1. **Probar login** con las credenciales de prueba
2. **Verificar autorización** en endpoints protegidos
3. **Confirmar roles** funcionando correctamente
4. **Implementar frontend** para consumir la API

## ?? Troubleshooting

### Si el login falla:
- Verificar que la base de datos tenga los usuarios semilla
- Confirmar que BCrypt esté funcionando correctamente
- Revisar que `IUserRepository` cargue la navegación `Rol` con `.Include(u => u.Rol)`

### Si la autorización falla:
- Verificar que el token se envíe en el header correcto
- Confirmar que el claim `Role` contenga el nombre del rol y no el ID
- Revisar la configuración de `TokenValidationParameters`