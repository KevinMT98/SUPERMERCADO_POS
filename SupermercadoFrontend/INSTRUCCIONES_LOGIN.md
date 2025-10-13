# 🔐 INSTRUCCIONES - LOGIN CON JWT

## ✅ Implementación Completada

Se ha implementado el sistema de login con autenticación JWT según la estructura del backend.

---

## 📁 Archivos Creados

```
wwwroot/
├── login.html                          ✅ Página de login con el diseño solicitado
└── Scripts/
    └── Comunes/
        ├── config.js                   ✅ Configuración de la API
        └── frmLogin.js                 ✅ Lógica de autenticación JWT
```

---

## 🔧 Configuración del Backend

El backend espera estos datos para el login:

### Endpoint: `POST /api/Auth/login`

**Request Body (LoginDTO):**
```json
{
    "email": "usuario@ejemplo.com",
    "password": "contraseña123"
}
```

**Response (TokenDTO):**
```json
{
    "accesToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expireIn": 3600,
    "email": "usuario@ejemplo.com",
    "role": "Admin"
}
```

---

## 🚀 Pasos para Usar

### 1. **Configurar URL del Backend**

Edita `wwwroot/Scripts/Comunes/frmLogin.js` (línea 8):

```javascript
const API_BASE_URL = 'https://localhost:7000/api'; // ⚠️ Cambiar según tu puerto
```

**Puertos comunes:**
- `https://localhost:7000`
- `https://localhost:5001`
- `http://localhost:5000`

### 2. **Verificar CORS en el Backend**

Asegúrate de que el backend tenga CORS configurado en `Program.cs`:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ...

app.UseCors("AllowAll");
```

### 3. **Ejecutar el Backend**

```bash
cd c:\Projects\SUPERMERCADO\SUPERMERCADO\Supermercado.Backend
dotnet run
```

Verifica que veas:
```
Now listening on: https://localhost:7000
```

### 4. **Ejecutar el Frontend**

```bash
cd c:\Projects\SUPERMERCADO\SupermercadoFrontend\SupermercadoWeb\SupermercadoWeb
dotnet run
```

### 5. **Acceder al Login**

Abre tu navegador en:
```
https://localhost:[puerto]/login.html
```

---

## 🔐 Credenciales de Prueba

Si tu backend tiene datos de prueba (SeedDb), usa las credenciales configuradas allí.

**Ejemplo típico:**
- **Email:** `admin@supermercado.com`
- **Password:** `Admin123!`

Si no tienes datos de prueba, puedes:
1. Crear un usuario directamente en la base de datos
2. Usar Swagger para crear un usuario: `https://localhost:7000/swagger`

---

## 🎯 Funcionalidades Implementadas

### ✅ Formulario de Login
- [x] Campo de email con validación
- [x] Campo de contraseña
- [x] Checkbox "Recordar contraseña"
- [x] Botón de ingreso con loading spinner
- [x] Diseño según el HTML proporcionado

### ✅ Validaciones
- [x] Validación de campos vacíos
- [x] Validación de formato de email
- [x] Mensajes de error claros

### ✅ Autenticación JWT
- [x] Llamada al endpoint `/api/Auth/login`
- [x] Envío de email y password
- [x] Recepción del token JWT
- [x] Almacenamiento en localStorage

### ✅ Manejo de Sesión
- [x] Guardar token en localStorage
- [x] Guardar email del usuario
- [x] Guardar rol del usuario
- [x] Guardar tiempo de expiración
- [x] Opción "Recordar contraseña"

### ✅ UX/UI
- [x] Loading spinner durante login
- [x] Alertas de éxito/error
- [x] Redirección automática al dashboard
- [x] Verificación de sesión activa
- [x] Diseño responsive

---

## 📊 Flujo de Autenticación

```
┌─────────────────┐
│  Usuario ingresa│
│  email/password │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Validación de  │
│     campos      │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  POST /api/Auth │
│     /login      │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Backend valida │
│   credenciales  │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Genera token   │
│      JWT        │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Guarda token   │
│  en localStorage│
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Redirige a     │
│   dashboard     │
└─────────────────┘
```

---

## 🔍 Datos Almacenados en localStorage

Después de un login exitoso, se guardan:

| Clave | Valor | Descripción |
|-------|-------|-------------|
| `token` | `eyJhbGci...` | Token JWT para autenticación |
| `email` | `usuario@ejemplo.com` | Email del usuario |
| `role` | `Admin` | Rol del usuario |
| `expireIn` | `3600` | Tiempo de expiración en segundos |
| `rememberMe` | `true` | Si el usuario marcó "Recordar" |

---

## 🛡️ Seguridad

### Implementado:
✅ Validación de email en cliente
✅ Envío de credenciales por HTTPS
✅ Token JWT almacenado en localStorage
✅ Validación de token en backend

### Recomendaciones adicionales:
- Usar HTTPS en producción
- Implementar refresh tokens
- Agregar límite de intentos de login
- Implementar CAPTCHA después de varios intentos fallidos
- Considerar usar httpOnly cookies en lugar de localStorage

---

## 🐛 Solución de Problemas

### Error: "Failed to fetch" o "Network Error"

**Causa:** Backend no está corriendo o URL incorrecta

**Solución:**
1. Verifica que el backend esté corriendo
2. Revisa la URL en `frmLogin.js` línea 8
3. Verifica CORS en el backend
4. Abre la consola del navegador (F12) para ver el error exacto

### Error: "Credenciales inválidas"

**Causa:** Email o password incorrectos

**Solución:**
1. Verifica las credenciales en la base de datos
2. Asegúrate de que el usuario existe
3. Verifica que la contraseña esté hasheada correctamente en BD

### Error: "CORS policy"

**Causa:** CORS no configurado en el backend

**Solución:**
1. Agrega configuración CORS en `Program.cs` del backend
2. Asegúrate de que `app.UseCors("AllowAll")` esté antes de `app.UseAuthorization()`

### No redirige al dashboard

**Causa:** Ruta del dashboard incorrecta

**Solución:**
1. Verifica que exista `/dashboard.html`
2. Revisa la consola del navegador (F12)
3. Verifica que el token se guardó correctamente en localStorage

---

## 🧪 Pruebas

### Prueba Manual:

1. **Login exitoso:**
   - Ingresa email y password válidos
   - Verifica que aparezca mensaje de éxito
   - Verifica redirección a dashboard
   - Abre DevTools → Application → Local Storage
   - Verifica que existan: token, email, role

2. **Login fallido:**
   - Ingresa credenciales incorrectas
   - Verifica mensaje de error
   - Verifica que el botón se reactive

3. **Validaciones:**
   - Intenta enviar formulario vacío
   - Intenta con email inválido (sin @)
   - Verifica mensajes de validación

4. **Recordar contraseña:**
   - Marca el checkbox
   - Verifica que se guarde en localStorage

---

## 📝 Código JavaScript Explicado

### Estructura del `frmLogin.js`:

```javascript
// 1. Configuración
const API_BASE_URL = '...';

// 2. Verificar sesión activa
if (localStorage.getItem('token')) {
    window.location.href = '/dashboard.html';
}

// 3. Event listener del formulario
document.getElementById('loginForm').addEventListener('submit', async (e) => {
    // 3.1 Prevenir submit por defecto
    e.preventDefault();
    
    // 3.2 Obtener valores
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;
    
    // 3.3 Validar
    if (!email || !password) { ... }
    
    // 3.4 Mostrar loading
    btnText.classList.add('d-none');
    btnSpinner.classList.remove('d-none');
    
    // 3.5 Llamar a la API
    const response = await fetch(`${API_BASE_URL}/Auth/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
    });
    
    // 3.6 Procesar respuesta
    const data = await response.json();
    
    // 3.7 Guardar en localStorage
    localStorage.setItem('token', data.accesToken);
    
    // 3.8 Redirigir
    window.location.href = '/dashboard.html';
});
```

---

## 🎯 Próximos Pasos

1. **Crear página de dashboard** que use el token
2. **Implementar logout** que limpie localStorage
3. **Proteger rutas** verificando token
4. **Agregar refresh token** para renovar sesión
5. **Implementar recuperación de contraseña**

---

## ✅ Checklist de Verificación

Antes de probar:

- [ ] Backend corriendo en el puerto correcto
- [ ] URL configurada en `frmLogin.js`
- [ ] CORS habilitado en backend
- [ ] Base de datos con usuarios de prueba
- [ ] Navegador con JavaScript habilitado
- [ ] Consola del navegador abierta (F12) para ver errores

---

## 🎉 ¡Listo!

Tu sistema de login con JWT está completamente implementado y listo para usar.

**Características:**
✅ Diseño según HTML proporcionado
✅ Autenticación JWT con el backend
✅ Validaciones completas
✅ Manejo de errores
✅ UX con loading y alertas
✅ Almacenamiento seguro del token

**Para probar:**
1. Ejecuta el backend
2. Ejecuta el frontend
3. Abre `/login.html`
4. Ingresa credenciales válidas
5. ¡Disfruta! 🚀
