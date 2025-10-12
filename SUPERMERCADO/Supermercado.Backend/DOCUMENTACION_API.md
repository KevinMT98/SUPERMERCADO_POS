# Documentación API - Supermercado POS

## Convenciones Generales

### Autenticación
- **Método**: JWT (JSON Web Token)
- **Header**: `Authorization: Bearer {token}`
- **Expiración**: Configurado en `appsettings.json` (por defecto 60 minutos)

### Estructura de Errores
Todos los errores siguen esta estructura:

```json
{
  "code": "ERROR_CODE",
  "message": "Mensaje descriptivo del error",
  "details": {},
  "timestamp": "2025-10-12T00:00:00Z"
}
```

#### Errores de Validación
```json
{
  "code": "VALIDATION_ERROR",
  "message": "Error de validación en los datos enviados",
  "errors": {
    "Email": ["El email es obligatorio"],
    "Password": ["La contraseña es obligatoria"]
  },
  "timestamp": "2025-10-12T00:00:00Z"
}
```

### Paginación
Todas las respuestas paginadas incluyen:

**Estructura del Body:**
```json
{
  "items": [...],
  "page": 1,
  "pageSize": 10,
  "total": 100,
  "totalPages": 10,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

**Header de Respuesta:**
- `X-Total-Count`: Total de elementos

**Parámetros de Query:**
- `page`: Número de página (base 1, por defecto 1)
- `pageSize`: Tamaño de página (por defecto 10, máximo 100)
- `sortBy`: Campo para ordenar
- `sortOrder`: Dirección (asc/desc)
- `search`: Término de búsqueda

### CORS
Configurado para permitir peticiones desde Visual FoxPro:
- Permite cualquier origen
- Permite todos los métodos HTTP
- Permite todos los headers
- Expone header `X-Total-Count`

---

## Endpoints de Autenticación

### POST /auth/login
Autentica un usuario y genera un token JWT.

**Request Body:**
```json
{
  "email": "admin@supermercado.com",
  "password": "Admin123"
}
```

**Response 200 OK:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600
}
```

**Response 400 Bad Request:**
```json
{
  "code": "VALIDATION_ERROR",
  "message": "Error de validación en los datos enviados",
  "errors": {
    "Email": ["El email es obligatorio"]
  },
  "timestamp": "2025-10-12T00:00:00Z"
}
```

**Response 401 Unauthorized:**
```json
{
  "code": "INVALID_CREDENTIALS",
  "message": "Las credenciales proporcionadas son incorrectas. Verifica tu email y contraseña.",
  "timestamp": "2025-10-12T00:00:00Z"
}
```

**Códigos de Error:**
- `INVALID_CREDENTIALS`: Email o contraseña incorrectos
- `USER_INACTIVE`: Usuario inactivo

---

### POST /auth/logout
Cierra la sesión del usuario (requiere autenticación).

**Headers:**
```
Authorization: Bearer {token}
```

**Response 204 No Content**

**Response 401 Unauthorized:**
```json
{
  "code": "UNAUTHORIZED",
  "message": "Token inválido o expirado",
  "timestamp": "2025-10-12T00:00:00Z"
}
```

---

### POST /auth/register
Registra un nuevo usuario en el sistema.

**Request Body:**
```json
{
  "nombreUsuario": "usuario1",
  "password": "User123",
  "confirmPassword": "User123",
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan.perez@supermercado.com",
  "rolId": 2
}
```

**Response 201 Created:**
```json
{
  "usuarioId": 1,
  "nombreUsuario": "usuario1",
  "nombre": "Juan",
  "apellido": "Pérez",
  "email": "juan.perez@supermercado.com",
  "rol": "User"
}
```

**Response 400 Bad Request:**
```json
{
  "code": "USERNAME_EXISTS",
  "message": "El nombre de usuario ya está en uso",
  "timestamp": "2025-10-12T00:00:00Z"
}
```

**Códigos de Error:**
- `USERNAME_EXISTS`: Nombre de usuario duplicado
- `EMAIL_EXISTS`: Email duplicado
- `INVALID_ROLE`: Rol no existe

---

## Uso del Token

Una vez autenticado, incluye el token en todas las peticiones protegidas:

**Header:**
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Ejemplo en cURL:**
```bash
curl -X GET "http://localhost:5000/api/Producto" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
```

**Ejemplo en JavaScript:**
```javascript
fetch('http://localhost:5000/api/Producto', {
  headers: {
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  }
})
```

**Ejemplo en Visual FoxPro:**
```foxpro
LOCAL loHTTP, lcToken, lcResponse
loHTTP = CREATEOBJECT("WinHttp.WinHttpRequest.5.1")
lcToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

loHTTP.Open("GET", "http://localhost:5000/api/Producto", .F.)
loHTTP.SetRequestHeader("Authorization", "Bearer " + lcToken)
loHTTP.SetRequestHeader("Content-Type", "application/json")
loHTTP.Send()

lcResponse = loHTTP.ResponseText
```

---

## Usuarios de Prueba

| Usuario | Email | Contraseña | Rol | Estado |
|---------|-------|------------|-----|--------|
| admin | admin@supermercado.com | Admin123 | Admin | Activo |
| superadmin | superadmin@supermercado.com | Super123 | Admin | Activo |
| usuario1 | juan.perez@supermercado.com | User123 | User | Activo |
| usuario2 | maria.garcia@supermercado.com | User123 | User | Activo |
| usuario_inactivo | inactivo@supermercado.com | Inactive123 | User | Inactivo |

---

## Ejemplo de Flujo Completo

### 1. Login
```bash
POST /auth/login
{
  "email": "admin@supermercado.com",
  "password": "Admin123"
}
```

**Respuesta:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresIn": 3600
}
```

### 2. Usar Token en Peticiones
```bash
GET /api/Producto
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 3. Logout
```bash
POST /auth/logout
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

**Respuesta:** 204 No Content

---

## Notas Importantes

1. **Expiración del Token**: El token expira después del tiempo configurado (por defecto 60 minutos). El cliente debe manejar la renovación.

2. **Seguridad**: 
   - Cambia la `SecretKey` en producción
   - Usa HTTPS en producción
   - No compartas tokens

3. **CORS**: Configurado para desarrollo. En producción, especifica los orígenes permitidos.

4. **Paginación**: Siempre verifica el header `X-Total-Count` para conocer el total de elementos.

5. **Errores**: Todos los errores incluyen un `code` para identificación programática y un `message` para mostrar al usuario.
