# Documentaci�n: Manejo de Usuario y Rol en AuthUnitOfWork

## Cambios Realizados

### 1. **Correcci�n del Namespace**
- **Antes:** `namespace Supermercado.Backend.Repositories.Implementations;`
- **Despu�s:** `namespace Supermercado.Backend.UnitsOfWork.Implementations;`
- **Raz�n:** El archivo debe estar en el namespace correcto seg�n su ubicaci�n.

### 2. **Correcci�n del Manejo de Roles**
- **Problema:** Se intentaba pasar `user.Rol` (objeto Rol) como string al m�todo `GenerateJwtToken`
- **Soluci�n:** Se extrae el nombre del rol: `user.Rol?.nombre ?? "User"`
- **Beneficio:** El claim de rol en el JWT contiene el nombre del rol (Admin, User), no el ID num�rico.

### 3. **Correcci�n de Propiedades del Usuario**
- **Problema:** Se usaban propiedades inexistentes (`user.Email`, `user.Role`)
- **Soluci�n:** Se usan las propiedades correctas de la entidad Usuario:
  - `user.email` (propiedad real de la entidad)
  - `user.Rol?.nombre` (navegaci�n al rol y su nombre)

### 4. **Mejoras en la Generaci�n del Token**
- **Agregado:** Claim `NameIdentifier` con el ID del usuario
- **Agregado:** Claim `Iat` (issued at) con timestamp UTC
- **Mejorado:** Uso de UTC en lugar de hora local (`DateTime.UtcNow`)
- **Beneficio:** Mejores pr�cticas de seguridad y compatibilidad global

## Estructura de Claims en el JWT

```csharp
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Email, email),                    // Email del usuario
    new Claim(ClaimTypes.Role, roleName),                  // Nombre del rol (Admin, User)
    new Claim(ClaimTypes.NameIdentifier, userId),          // ID del usuario
    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Token ID �nico
    new Claim(JwtRegisteredClaimNames.Iat, timestamp)      // Timestamp de emisi�n
};
```

## Flujo de Autenticaci�n Corregido

### 1. **Validaci�n de Usuario**
```csharp
var userResponse = await _usuarioRepository.GetByEmailAsync(dto.Email);
if (!userResponse.WasSuccess || userResponse.Result == null)
    return CredencialesInvalidas();
```

### 2. **Verificaci�n de Estado Activo**
```csharp
if (!user.activo)
    return UsuarioInactivo();
```

### 3. **Validaci�n de Contrase�a**
```csharp
var isValidPassword = await _usuarioRepository.ValidatePasswordAsync(user, dto.Password);
if (!isValidPassword)
    return CredencialesInvalidas();
```

### 4. **Extracci�n del Rol**
```csharp
var roleName = user.Rol?.nombre ?? "User"; // Usa el nombre del rol, no el ID
```

### 5. **Generaci�n del Token**
```csharp
var token = GenerateJwtToken(user.email ?? string.Empty, roleName, user.usuario_id.ToString());
```

## Relaci�n Usuario-Rol Correcta

### En la Entidad Usuario:
```csharp
public class Usuario
{
    // ... otras propiedades ...
    
    [Column("FK_rol_id")]
    public int FK_rol_id { get; set; }          // Clave for�nea
    
    [ForeignKey("FK_rol_id")]
    public Rol? Rol { get; set; }               // Navegaci�n al rol
}
```

### En el Repository (IUserRepository):
```csharp
public async Task<ActionResponse<Usuario>> GetByEmailAsync(string email)
{
    var usuario = await _context.Usuarios
        .Include(u => u.Rol)  // IMPORTANTE: Cargar la navegaci�n al rol
        .FirstOrDefaultAsync(u => u.email == email);
    // ...
}
```

## Uso en Controladores con Autorizaci�n

### Autorizaci�n por Rol:
```csharp
[Authorize(Roles = "Admin")]           // Solo usuarios con rol Admin
[Authorize(Roles = "Admin,User")]      // Usuarios con rol Admin o User
[Authorize]                           // Cualquier usuario autenticado
```

### Obtener Claims del Usuario Autenticado:
```csharp
public async Task<IActionResult> MiMetodo()
{
    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
    var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
    // Usar los claims seg�n sea necesario...
}
```

## Configuraci�n Requerida en Program.cs

Para que funcione correctamente, aseg�rate de tener en `Program.cs`:

```csharp
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

// ...

app.UseAuthentication();
app.UseAuthorization();
```

## Beneficios de los Cambios

1. **Seguridad Mejorada:** Claims correctos con nombres de rol en lugar de IDs
2. **Compatibilidad:** Funciona correctamente con `[Authorize(Roles = "Admin")]`
3. **Mantenibilidad:** C�digo m�s claro y bien documentado
4. **Est�ndares:** Uso de UTC y claims est�ndar de JWT
5. **Robustez:** Manejo de nulos y validaciones apropiadas

## Pruebas Recomendadas

1. **Login exitoso** con usuario activo
2. **Login fallido** con credenciales incorrectas
3. **Login fallido** con usuario inactivo
4. **Verificaci�n de claims** en el token generado
5. **Autorizaci�n por roles** en endpoints protegidos