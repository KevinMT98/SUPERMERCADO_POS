# 📋 INSTRUCCIONES DE INSTALACIÓN Y CONFIGURACIÓN

## Frontend - Supermercado POS

### 🎯 Resumen

Se ha implementado un frontend completo con arquitectura limpia basada en el backend .NET. El proyecto utiliza:
- **HTML5** para estructura
- **JavaScript ES6+** para lógica
- **CSS3** para estilos
- **Bootstrap 5** para UI responsivo

---

## 📁 Estructura Implementada

```
SupermercadoWeb/wwwroot/
│
├── 📄 HTML Pages (Páginas principales)
│   ├── login.html              ✅ Página de inicio de sesión
│   ├── dashboard.html          ✅ Dashboard con estadísticas
│   ├── productos.html          ✅ Gestión de productos
│   ├── categorias.html         ✅ Gestión de categorías
│   ├── tarifas-iva.html       ✅ Gestión de tarifas IVA
│   └── usuarios.html           ✅ Gestión de usuarios
│
├── 📂 js/ (JavaScript)
│   ├── config.js               ✅ Configuración central
│   │
│   ├── 📂 api/
│   │   └── apiClient.js        ✅ Cliente HTTP (capa de comunicación)
│   │
│   ├── 📂 services/ (Capa de negocio - Unit of Work)
│   │   ├── authService.js      ✅ Autenticación
│   │   ├── productoService.js  ✅ Productos
│   │   ├── categoriaService.js ✅ Categorías
│   │   ├── tarifaIvaService.js ✅ Tarifas IVA
│   │   ├── usuarioService.js   ✅ Usuarios
│   │   └── rolService.js       ✅ Roles
│   │
│   ├── 📂 utils/ (Utilidades)
│   │   ├── storage.js          ✅ Manejo de localStorage
│   │   ├── validators.js       ✅ Validaciones
│   │   └── helpers.js          ✅ Funciones auxiliares
│   │
│   └── 📂 pages/ (Lógica por página)
│       └── productos.js        ✅ Lógica de productos
│
├── 📂 css/
│   └── site.css                ✅ Estilos personalizados
│
├── 📂 lib/ (Librerías - ya existentes)
│   ├── bootstrap/
│   ├── jquery/
│   └── ...
│
└── 📄 README.md                ✅ Documentación completa
```

---

## 🚀 Pasos de Configuración

### 1. **Verificar Estructura de Archivos**

Todos los archivos se han creado en la ubicación correcta. Verifica que existan:

```
✅ wwwroot/login.html
✅ wwwroot/dashboard.html
✅ wwwroot/productos.html
✅ wwwroot/categorias.html
✅ wwwroot/tarifas-iva.html
✅ wwwroot/usuarios.html
✅ wwwroot/js/config.js
✅ wwwroot/js/api/apiClient.js
✅ wwwroot/js/services/*.js (6 archivos)
✅ wwwroot/js/utils/*.js (3 archivos)
✅ wwwroot/js/pages/productos.js
✅ wwwroot/css/site.css
```

### 2. **Configurar URL del Backend**

Edita `wwwroot/js/config.js` y actualiza la URL del backend:

```javascript
const AppConfig = {
    API_BASE_URL: 'https://localhost:7000/api',  // ⚠️ Cambiar según tu puerto
    // ... resto de configuración
};
```

**Nota:** Verifica el puerto en el que corre tu backend. Puede ser:
- `https://localhost:7000`
- `https://localhost:5001`
- `http://localhost:5000`

### 3. **Verificar CORS en el Backend**

Asegúrate de que el backend tenga configurado CORS correctamente en `Program.cs`:

```csharp
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

// ...

app.UseCors("AllowAll");
```

### 4. **Ejecutar el Backend**

Antes de probar el frontend, asegúrate de que el backend esté corriendo:

```bash
cd c:\Projects\SUPERMERCADO\SUPERMERCADO\Supermercado.Backend
dotnet run
```

Verifica que veas un mensaje similar a:
```
Now listening on: https://localhost:7000
```

### 5. **Ejecutar el Frontend**

Desde Visual Studio:
1. Abre la solución `SupermercadoWeb.sln`
2. Presiona F5 o haz clic en "Run"
3. El navegador debería abrirse automáticamente

O desde la línea de comandos:
```bash
cd c:\Projects\SUPERMERCADO\SupermercadoFrontend\SupermercadoWeb\SupermercadoWeb
dotnet run
```

### 6. **Acceder a la Aplicación**

1. Abre tu navegador en: `https://localhost:[puerto]/login.html`
2. Usa las credenciales de un usuario creado en el backend
3. Tras el login exitoso, serás redirigido al dashboard

---

## 🔐 Credenciales de Prueba

Si el backend tiene datos de prueba (SeedDb), usa:
- **Usuario:** (el que hayas configurado en SeedDb)
- **Contraseña:** (la que hayas configurado en SeedDb)

Si no tienes datos de prueba, crea un usuario directamente en la base de datos o mediante Swagger.

---

## 🎨 Características Implementadas

### ✅ Arquitectura Limpia
- **Separación de responsabilidades** en capas (API, Services, Utils, Pages)
- **Patrón Repository** en `apiClient.js`
- **Patrón Unit of Work** en servicios
- **DRY** - Código reutilizable

### ✅ Autenticación y Seguridad
- Login con JWT
- Almacenamiento seguro de tokens
- Protección de rutas
- Redirección automática si no está autenticado
- Manejo de sesión expirada

### ✅ Gestión Completa de Entidades
- **Productos:** CRUD completo con validaciones
- **Categorías:** CRUD completo
- **Tarifas IVA:** CRUD completo
- **Usuarios:** CRUD completo

### ✅ Funcionalidades UX
- Búsqueda en tiempo real con debounce
- Filtros por categoría, estado, etc.
- Modals para crear/editar
- Confirmaciones antes de eliminar
- Notificaciones toast
- Loading spinners
- Validación de formularios
- Mensajes de error claros

### ✅ Diseño Responsive
- Compatible con móviles, tablets y desktop
- Tablas con scroll horizontal en móviles
- Navbar colapsable
- Grid system de Bootstrap

---

## 🔧 Personalización

### Cambiar Colores

Edita las variables CSS en `wwwroot/css/site.css`:

```css
:root {
    --primary-color: #0d6efd;     /* Azul principal */
    --secondary-color: #6c757d;   /* Gris */
    --success-color: #198754;     /* Verde */
    --danger-color: #dc3545;      /* Rojo */
    /* ... */
}
```

### Agregar Nuevo Módulo

1. **Crear servicio** en `js/services/nuevoService.js`
2. **Crear página HTML** `nuevo.html`
3. **Agregar enlace** en navbar de todas las páginas
4. **Implementar lógica** similar a `productos.js`

---

## 🐛 Solución de Problemas

### Error: "Failed to fetch" o "Network Error"

**Causa:** El backend no está corriendo o la URL es incorrecta

**Solución:**
1. Verifica que el backend esté corriendo
2. Revisa la URL en `js/config.js`
3. Verifica CORS en el backend

### Error: "401 Unauthorized"

**Causa:** Token inválido o expirado

**Solución:**
1. Cierra sesión y vuelve a iniciar
2. Verifica que el token se esté enviando correctamente
3. Revisa la configuración JWT en el backend

### Error: "404 Not Found"

**Causa:** Endpoint incorrecto

**Solución:**
1. Verifica los endpoints en los servicios
2. Compara con los controladores del backend
3. Revisa la consola del navegador (F12)

### Los estilos no se aplican

**Causa:** Ruta incorrecta a CSS

**Solución:**
1. Verifica que `site.css` exista en `wwwroot/css/`
2. Revisa la ruta en el `<link>` del HTML
3. Limpia caché del navegador (Ctrl + F5)

### Bootstrap no funciona

**Causa:** Librerías no cargadas

**Solución:**
1. Verifica que existan en `wwwroot/lib/bootstrap/`
2. Revisa las rutas en los `<script>` tags
3. Abre la consola del navegador para ver errores

---

## 📊 Flujo de Datos

```
┌─────────────┐
│   Usuario   │
└──────┬──────┘
       │
       ▼
┌─────────────────┐
│  Página HTML    │ (login.html, productos.html, etc.)
└──────┬──────────┘
       │
       ▼
┌─────────────────┐
│   Page JS       │ (productos.js, etc.)
└──────┬──────────┘
       │
       ▼
┌─────────────────┐
│   Service       │ (productoService.js, etc.)
└──────┬──────────┘
       │
       ▼
┌─────────────────┐
│  API Client     │ (apiClient.js)
└──────┬──────────┘
       │
       ▼
┌─────────────────┐
│  Backend API    │ (ASP.NET Core)
└─────────────────┘
```

---

## 📚 Documentación Adicional

Para más detalles sobre la arquitectura y uso, consulta:
- `wwwroot/README.md` - Documentación completa del frontend
- Comentarios JSDoc en cada archivo JavaScript
- Comentarios en línea explicando lógica compleja

---

## ✅ Checklist de Verificación

Antes de usar la aplicación, verifica:

- [ ] Backend corriendo en el puerto correcto
- [ ] URL del backend configurada en `config.js`
- [ ] CORS habilitado en el backend
- [ ] Base de datos creada y con datos de prueba
- [ ] Todos los archivos JavaScript en su lugar
- [ ] Bootstrap y jQuery cargados correctamente
- [ ] Navegador con JavaScript habilitado
- [ ] Consola del navegador sin errores (F12)

---

## 🎯 Próximos Pasos

1. **Probar cada módulo:**
   - Login
   - Dashboard
   - Productos (CRUD completo)
   - Categorías (CRUD completo)
   - Tarifas IVA (CRUD completo)
   - Usuarios (CRUD completo)

2. **Personalizar según necesidades:**
   - Agregar más validaciones
   - Implementar paginación en tablas grandes
   - Agregar reportes
   - Implementar módulo de ventas/POS

3. **Optimizaciones:**
   - Implementar caché de datos
   - Agregar service workers para PWA
   - Optimizar imágenes y assets
   - Implementar lazy loading

---

## 📞 Soporte

Si encuentras problemas:

1. **Revisa la consola del navegador** (F12 → Console)
2. **Revisa la pestaña Network** (F12 → Network) para ver peticiones HTTP
3. **Verifica los logs del backend**
4. **Consulta el README.md** en wwwroot/

---

## 🎉 ¡Listo para Usar!

Tu frontend está completamente implementado y listo para conectarse con el backend. Sigue los pasos de configuración y estarás operativo en minutos.

**¡Buena suerte con tu sistema POS de Supermercado! 🛒**
