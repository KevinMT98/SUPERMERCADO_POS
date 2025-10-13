# SUPERMERCADO POS - Frontend

## 📋 Descripción

Frontend del sistema de punto de venta (POS) para supermercado, construido con HTML, JavaScript, CSS y Bootstrap 5. Implementa una arquitectura limpia con separación de responsabilidades similar al backend en .NET.

## 🏗️ Arquitectura

La aplicación sigue una arquitectura en capas inspirada en el patrón Repository y Unit of Work del backend:

```
wwwroot/
├── js/
│   ├── config.js                 # Configuración central
│   ├── api/
│   │   └── apiClient.js          # Cliente HTTP (capa de comunicación)
│   ├── services/                 # Servicios de negocio (Unit of Work)
│   │   ├── authService.js
│   │   ├── productoService.js
│   │   ├── categoriaService.js
│   │   ├── tarifaIvaService.js
│   │   └── usuarioService.js
│   ├── utils/                    # Utilidades
│   │   ├── storage.js            # Manejo de localStorage
│   │   ├── validators.js         # Validaciones
│   │   └── helpers.js            # Funciones auxiliares
│   └── pages/                    # Lógica específica por página
│       └── productos.js
├── css/
│   └── site.css                  # Estilos personalizados
├── lib/                          # Librerías (Bootstrap, jQuery)
├── login.html                    # Página de login
├── dashboard.html                # Dashboard principal
├── productos.html                # Gestión de productos
├── categorias.html               # Gestión de categorías
├── tarifas-iva.html             # Gestión de tarifas IVA
└── usuarios.html                 # Gestión de usuarios
```

## 🔄 Flujo de Datos

```
Usuario → Página HTML → Page JS → Service → API Client → Backend API
                                                              ↓
Usuario ← Página HTML ← Page JS ← Service ← API Client ← Backend API
```

## 📦 Componentes Principales

### 1. **config.js** - Configuración Central
- URL del API Backend
- Claves de localStorage
- Mensajes de la aplicación
- Configuraciones de validación

### 2. **apiClient.js** - Cliente HTTP
Maneja todas las comunicaciones con el backend:
- Métodos: GET, POST, PUT, DELETE
- Manejo de autenticación JWT
- Manejo centralizado de errores
- Interceptor de respuestas

### 3. **Services** - Capa de Negocio
Equivalentes a los UnitsOfWork del backend:

#### authService.js
- `login(nombreUsuario, password)` - Iniciar sesión
- `logout()` - Cerrar sesión
- `getCurrentUser()` - Obtener usuario actual
- `isAuthenticated()` - Verificar autenticación
- `requireAuth()` - Proteger páginas

#### productoService.js
- `getAll()` - Obtener todos los productos
- `getById(id)` - Obtener producto por ID
- `create(producto)` - Crear producto
- `update(id, producto)` - Actualizar producto
- `delete(id)` - Eliminar producto
- `search(searchTerm)` - Buscar productos
- `getProductosConStockBajo()` - Productos con stock bajo

#### categoriaService.js
- CRUD completo para categorías

#### tarifaIvaService.js
- CRUD completo para tarifas de IVA

#### usuarioService.js
- CRUD completo para usuarios

### 4. **Utils** - Utilidades

#### storage.js
- Manejo centralizado de localStorage
- Métodos específicos para token y usuario
- Serialización/deserialización automática

#### validators.js
- Validaciones de formularios
- Validación de email, contraseñas, números
- Validación completa de formularios HTML
- Mostrar errores en UI

#### helpers.js
- `formatCurrency(amount)` - Formatear moneda
- `formatDate(date)` - Formatear fechas
- `showToast(message, type)` - Mostrar notificaciones
- `showConfirmModal(title, message, onConfirm)` - Modal de confirmación
- `showLoading(show)` - Mostrar/ocultar loading
- `debounce(func, wait)` - Optimizar búsquedas

## 🔐 Autenticación

El sistema utiliza JWT (JSON Web Tokens) para autenticación:

1. Usuario ingresa credenciales en `login.html`
2. `authService.login()` envía credenciales al backend
3. Backend retorna token JWT
4. Token se guarda en localStorage
5. Todas las peticiones subsiguientes incluyen el token en header `Authorization: Bearer {token}`
6. Si el token expira o es inválido, se redirige automáticamente al login

## 📄 Páginas HTML

### login.html
- Formulario de inicio de sesión
- Validación de credenciales
- Opción "Recordarme"
- Redirige a dashboard tras login exitoso

### dashboard.html
- Estadísticas generales (productos, stock bajo, categorías, usuarios)
- Lista de productos con stock bajo
- Accesos rápidos a módulos principales

### productos.html
- Tabla de productos con paginación
- Búsqueda y filtros (por categoría, estado)
- Modal para crear/editar productos
- Validación de formularios
- Eliminación con confirmación

## 🎨 Estilos CSS

El archivo `site.css` incluye:
- Variables CSS para colores y estilos
- Estilos para navbar, cards, tables, forms
- Animaciones y transiciones
- Diseño responsive
- Estilos para modals y toasts

## 🚀 Uso

### Configuración Inicial

1. **Actualizar URL del Backend** en `js/config.js`:
```javascript
API_BASE_URL: 'https://localhost:7000/api'
```

2. **Incluir scripts en orden correcto**:
```html
<!-- Bootstrap -->
<script src="lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>

<!-- Configuración -->
<script src="js/config.js"></script>

<!-- Utilidades -->
<script src="js/utils/storage.js"></script>
<script src="js/utils/validators.js"></script>
<script src="js/utils/helpers.js"></script>

<!-- API Client -->
<script src="js/api/apiClient.js"></script>

<!-- Servicios -->
<script src="js/services/authService.js"></script>
<script src="js/services/productoService.js"></script>
<!-- ... otros servicios según necesidad -->

<!-- Lógica de página -->
<script src="js/pages/productos.js"></script>
```

### Proteger Páginas

```javascript
// Al inicio de cada página que requiera autenticación
authService.requireAuth();
```

### Usar Servicios

```javascript
// Obtener productos
const productos = await productoService.getAll();

// Crear producto
const nuevoProducto = {
    codigoProducto: 'PROD001',
    codigoBarras: '1234567890',
    nombre: 'Producto Ejemplo',
    // ... otros campos
};
await productoService.create(nuevoProducto);

// Actualizar producto
await productoService.update(id, productoActualizado);

// Eliminar producto
await productoService.delete(id);
```

### Mostrar Notificaciones

```javascript
// Toast de éxito
Helpers.showToast('Operación exitosa', 'success');

// Toast de error
Helpers.showToast('Error al procesar', 'error');

// Modal de confirmación
Helpers.showConfirmModal(
    'Confirmar',
    '¿Está seguro?',
    () => {
        // Acción al confirmar
    }
);
```

## 🔧 Extensibilidad

### Agregar Nuevo Módulo

1. **Crear servicio** en `js/services/`:
```javascript
class NuevoService {
    constructor(apiClient) {
        this.apiClient = apiClient;
        this.endpoint = '/NuevoEndpoint';
    }
    
    async getAll() {
        return await this.apiClient.get(this.endpoint);
    }
    // ... otros métodos CRUD
}

window.nuevoService = new NuevoService(window.apiClient);
```

2. **Crear página HTML** con estructura similar a `productos.html`

3. **Crear lógica de página** en `js/pages/nuevo.js`

4. **Agregar enlace** en navbar de todas las páginas

## 📱 Responsive Design

La aplicación es completamente responsive gracias a Bootstrap 5:
- Grid system para layouts
- Componentes adaptativos
- Media queries en CSS personalizado
- Tablas con scroll horizontal en móviles

## 🔒 Seguridad

- Tokens JWT almacenados en localStorage
- Validación de entrada en cliente y servidor
- Sanitización de datos
- Protección de rutas con `requireAuth()`
- Manejo seguro de errores sin exponer información sensible

## 📊 Manejo de Errores

Todos los errores se manejan de forma centralizada en `apiClient.js`:
- Errores 401: Sesión expirada → Redirige a login
- Errores 403: Sin permisos → Muestra mensaje
- Errores 404: No encontrado → Muestra mensaje
- Errores 500: Error de servidor → Muestra mensaje genérico

## 🎯 Buenas Prácticas Implementadas

✅ Separación de responsabilidades (capas bien definidas)
✅ DRY (Don't Repeat Yourself) - Código reutilizable
✅ Naming conventions consistentes
✅ Manejo centralizado de errores
✅ Validaciones en cliente y servidor
✅ Código documentado con JSDoc
✅ Uso de async/await para código asíncrono limpio
✅ Debouncing en búsquedas para optimizar rendimiento
✅ Loading states para mejor UX
✅ Confirmaciones antes de acciones destructivas

## 🚧 Páginas Pendientes de Implementar

Las siguientes páginas siguen el mismo patrón que `productos.html`:

- **categorias.html** - Gestión de categorías de productos
- **tarifas-iva.html** - Gestión de tarifas de IVA
- **usuarios.html** - Gestión de usuarios del sistema

Cada una debe incluir:
- Tabla con datos
- Búsqueda y filtros
- Modal para crear/editar
- Validaciones
- Manejo de errores

## 📞 Soporte

Para dudas o problemas, revisar:
1. Consola del navegador (F12) para errores JavaScript
2. Network tab para errores de API
3. Verificar que el backend esté corriendo
4. Verificar configuración de CORS en backend
