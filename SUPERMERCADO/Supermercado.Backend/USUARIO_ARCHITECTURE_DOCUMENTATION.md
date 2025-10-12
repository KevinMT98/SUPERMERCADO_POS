# Documentaci�n: Maestro de Usuarios vs Autenticaci�n

## ? Estructura Implementada

Se ha separado correctamente el **maestro de usuarios** (CRUD) de la **autenticaci�n** (login/token).

### ?? Arquitectura de Usuarios

```
???????????????????????????????????????????????????????????????
?                    CAPA DE PRESENTACI�N                      ?
???????????????????????????????????????????????????????????????
?                                                              ?
?  UsuarioController (CRUD)        AuthController (Login)    ?
?  /api/usuario                    /auth/login                ?
?  - GET /api/usuario              - POST /auth/login         ?
?  - GET /api/usuario/{id}         - POST /auth/register     ?
?  - POST /api/usuario                                        ?
?  - PUT /api/usuario/{id}                                    ?
?  - DELETE /api/usuario/{id}                                 ?
?                                                              ?
???????????????????????????????????????????????????????????????
                   ?                       ?
???????????????????????????????????  ?????????????????????????
?   IUsuarioUnitOfWork             ?  ?  IAuthUnitOfWork      ?
?   UsuarioUnitOfWork              ?  ?  AuthUnitOfWork       ?
???????????????????????????????????  ?????????????????????????
                   ?                       ?
???????????????????????????????????  ?????????????????????????
?   IUsuarioRepository             ?  ?  IUserRepository      ?
?   UsuarioRepository              ?  ?  UserRepository       ?
?   - GetAsync()                   ?  ?  - GetByEmailAsync()  ?
?   - GetAsync(id)                 ?  ?  - CreateUserAsync()  ?
?   - AddAsync()                   ?  ?  - ValidatePassword() ?
?   - UpdateAsync()                ?  ?                       ?
?   - DeleteAsync()                ?  ?                       ?
?   - ExistsByNombreUsuario()      ?  ?                       ?
?   - ExistsByEmail()              ?  ?                       ?
???????????????????????????????????  ?????????????????????????
                   ?                       ?
                   ?????????????????????????
                               ?
                      ????????????????????
                      ?   DataContext    ?
                      ?   DbSet<Usuario> ?
                      ????????????????????
```

## ?? Separaci�n de Responsabilidades

### **IUserRepository / UserRepository** (Autenticaci�n)
- **Prop�sito:** Validaci�n de credenciales y autenticaci�n
- **M�todos:**
  - `GetByEmailAsync(string email)` - Buscar usuario por email (para login)
  - `ValidatePasswordAsync(Usuario, password)` - Validar contrase�a
  - `CreateUserAsync(Usuario)` - Crear usuario (registro p�blico)
- **Uso:** Solo para `AuthController` y `AuthUnitOfWork`

### **IUsuarioRepository / UsuarioRepository** (Maestro de Usuarios)
- **Prop�sito:** Gesti�n completa de usuarios (CRUD administrativo)
- **M�todos:**
  - `GetAsync()` - Listar todos los usuarios
  - `GetAsync(int id)` - Obtener usuario por ID
  - `AddAsync(Usuario)` - Crear usuario (admin)
  - `UpdateAsync(Usuario)` - Actualizar usuario
  - `DeleteAsync(int id)` - Eliminar usuario
  - `ExistsByNombreUsuarioAsync(string, int?)` - Validar unicidad
  - `ExistsByEmailAsync(string, int?)` - Validar unicidad
- **Uso:** Solo para `UsuarioController` y `UsuarioUnitOfWork`

## ?? DTOs Utilizados

### Para Autenticaci�n (`AuthController`)
```csharp
LoginDTO           // Login de usuario
TokenDTO           // Respuesta con token JWT
```

### Para Maestro de Usuarios (`UsuarioController`)
```csharp
UsuarioCreateDto   // Crear nuevo usuario
UsuarioUpdateDto   // Actualizar usuario existente
UsuarioDto         // Lectura de usuario
```

## ?? Autorizaci�n

### AuthController
```csharp
[AllowAnonymous]  // Login y registro son p�blicos
```

### UsuarioController
```csharp
[Authorize(Roles = "Admin")]  // Solo administradores
```

## ?? Endpoints Disponibles

### Autenticaci�n (P�blico)
```http
POST /auth/login
POST /auth/register
```

### Maestro de Usuarios (Solo Admin)
```http
GET    /api/usuario              # Listar usuarios
GET    /api/usuario/{id}         # Obtener usuario
POST   /api/usuario              # Crear usuario
PUT    /api/usuario/{id}         # Actualizar usuario
DELETE /api/usuario/{id}         # Eliminar usuario
PATCH  /api/usuario/{id}/estado  # Activar/Desactivar
```

## ?? Ejemplos de Uso

### 1. Login (P�blico)
```bash
POST /auth/login
{
  "email": "admin@supermercado.com",
  "password": "Admin123!"
}

# Respuesta:
{
  "accesToken": "eyJhbGciOiJIUzI1NiIs...",
  "expireIn": 3600,
  "email": "admin@supermercado.com",
  "role": "Admin"
}
```

### 2. Listar Usuarios (Requiere token Admin)
```bash
GET /api/usuario
Authorization: Bearer {token}

# Respuesta:
[
  {
    "usuario_id": 1,
    "nombre_usuario": "admin",
    "nombre": "Administrador",
    "apellido": "Sistema",
    "email": "admin@supermercado.com",
    "rol": {
      "rol_id": 1,
      "nombre": "Admin"
    },
    "activo": true
  }
]
```

### 3. Crear Usuario (Requiere token Admin)
```bash
POST /api/usuario
Authorization: Bearer {token}
{
  "nombreUsuario": "nuevousuario",
  "password": "Pass123!",
  "nombre": "Nuevo",
  "apellido": "Usuario",
  "email": "nuevo@supermercado.com",
  "rolId": 2
}
```

### 4. Actualizar Usuario (Requiere token Admin)
```bash
PUT /api/usuario/5
Authorization: Bearer {token}
{
  "usuarioId": 5,
  "nombreUsuario": "usuarioactualizado",
  "nombre": "Usuario",
  "apellido": "Actualizado",
  "email": "actualizado@supermercado.com",
  "rolId": 2,
  "activo": true
}
```

### 5. Cambiar Estado (Requiere token Admin)
```bash
PATCH /api/usuario/5/estado
Authorization: Bearer {token}
Content-Type: application/json

false  # Desactivar usuario
```

## ?? Seguridad

### Contrase�as
- Se hashean con **BCrypt** antes de guardar
- El hash se genera autom�ticamente en el repositorio
- No se exponen en respuestas de la API

### Validaciones
- **Nombre de usuario �nico** (validaci�n en create/update)
- **Email �nico** (validaci�n en create/update)
- **Rol v�lido** (debe existir en la tabla Rols)

### Autorizaci�n
- **AuthController:** Acceso p�blico
- **UsuarioController:** Solo usuarios con rol `Admin`

## ?? Registro en Program.cs

```csharp
// Autenticaci�n
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthUnitOfWork, AuthUnitOfWork>();

// Maestro de Usuarios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioUnitOfWork, UsuarioUnitOfWork>();
```

## ? Validaci�n Completa

### Repositorios Implementados
- ? `IUserRepository` / `UserRepository` (Autenticaci�n)
- ? `IUsuarioRepository` / `UsuarioRepository` (Maestro)

### Units of Work Implementados
- ? `IAuthUnitOfWork` / `AuthUnitOfWork` (Autenticaci�n)
- ? `IUsuarioUnitOfWork` / `UsuarioUnitOfWork` (Maestro)

### Controladores Implementados
- ? `AuthController` (Login, registro, token)
- ? `UsuarioController` (CRUD de usuarios)

### DTOs Implementados
- ? `LoginDTO`, `TokenDTO` (Autenticaci�n)
- ? `UsuarioCreateDto`, `UsuarioUpdateDto`, `UsuarioDto` (Maestro)

### Validaciones
- ? Unicidad de nombre de usuario
- ? Unicidad de email
- ? Hash de contrase�as con BCrypt
- ? Carga de navegaci�n `Rol` en consultas
- ? Autorizaci�n por roles

## ?? Resultado

El sistema ahora tiene dos APIs separadas y bien organizadas:
1. **API de Autenticaci�n** (`/auth/*`) - Login y registro
2. **API de Maestro de Usuarios** (`/api/usuario/*`) - Gesti�n administrativa de usuarios

Ambas comparten las mismas entidades pero tienen responsabilidades claramente separadas seg�n los principios SOLID y Clean Architecture.
