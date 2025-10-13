/**
 * SUPERMERCADO POS - Productos Page
 * Lógica específica para la página de gestión de productos
 */

// Variables globales
let productos = [];
let categorias = [];
let tarifasIva = [];
let currentProductoId = null;

// Proteger la página
authService.requireAuth();

// Mostrar información del usuario
const user = authService.getCurrentUser();
if (user) {
    document.getElementById('userInfo').textContent = `👤 ${user.nombreUsuario}`;
}

// Cerrar sesión
document.getElementById('btnLogout').addEventListener('click', async () => {
    await authService.logout();
});

// Inicializar página
async function init() {
    await loadCategorias();
    await loadTarifasIva();
    await loadProductos();
    setupEventListeners();
}

// Cargar categorías
async function loadCategorias() {
    try {
        categorias = await categoriaService.getAll();
        
        // Llenar select de categorías en el formulario
        const selectCategoria = document.getElementById('categoriaId');
        selectCategoria.innerHTML = '<option value="">Seleccione...</option>';
        categorias.forEach(cat => {
            selectCategoria.innerHTML += `<option value="${cat.categoria_Producto_Id}">${cat.descripcion}</option>`;
        });
        
        // Llenar filtro de categorías
        const filterCategoria = document.getElementById('filterCategoria');
        filterCategoria.innerHTML = '<option value="">Todas las categorías</option>';
        categorias.forEach(cat => {
            filterCategoria.innerHTML += `<option value="${cat.categoria_Producto_Id}">${cat.descripcion}</option>`;
        });
    } catch (error) {
        console.error('Error al cargar categorías:', error);
        Helpers.showToast('Error al cargar categorías', 'error');
    }
}

// Cargar tarifas de IVA
async function loadTarifasIva() {
    try {
        tarifasIva = await tarifaIvaService.getAll();
        
        // Llenar select de IVA
        const selectIva = document.getElementById('codigoIva');
        selectIva.innerHTML = '<option value="">Seleccione...</option>';
        tarifasIva.forEach(iva => {
            selectIva.innerHTML += `<option value="${iva.tarifaIvaId}">${iva.descripcion} (${iva.porcentaje}%)</option>`;
        });
    } catch (error) {
        console.error('Error al cargar tarifas de IVA:', error);
        Helpers.showToast('Error al cargar tarifas de IVA', 'error');
    }
}

// Cargar productos
async function loadProductos() {
    try {
        productos = await productoService.getAll();
        renderProductos(productos);
    } catch (error) {
        console.error('Error al cargar productos:', error);
        Helpers.showToast('Error al cargar productos', 'error');
        document.getElementById('productosTableBody').innerHTML = `
            <tr>
                <td colspan="8" class="text-center text-danger py-4">
                    Error al cargar productos
                </td>
            </tr>
        `;
    }
}

// Renderizar tabla de productos
function renderProductos(data) {
    const tbody = document.getElementById('productosTableBody');
    
    if (data.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center text-muted py-4">
                    No se encontraron productos
                </td>
            </tr>
        `;
        return;
    }
    
    tbody.innerHTML = data.map(producto => {
        const categoria = categorias.find(c => c.categoria_Producto_Id === producto.categoriaId);
        const stockClass = producto.stockActual < producto.stockMinimo ? 'text-danger fw-bold' : '';
        
        return `
            <tr>
                <td>${producto.codigoProducto}</td>
                <td>${producto.codigoBarras}</td>
                <td>${producto.nombre}</td>
                <td>${Helpers.formatCurrency(producto.precioUnitario)}</td>
                <td class="${stockClass}">${producto.stockActual} / ${producto.stockMinimo}</td>
                <td>${categoria ? categoria.descripcion : 'N/A'}</td>
                <td>
                    <span class="badge ${producto.activo ? 'bg-success' : 'bg-secondary'}">
                        ${producto.activo ? 'Activo' : 'Inactivo'}
                    </span>
                </td>
                <td>
                    <button class="btn btn-sm btn-primary" onclick="openEditModal(${producto.productoId})">
                        ✏️
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="deleteProducto(${producto.productoId})">
                        🗑️
                    </button>
                </td>
            </tr>
        `;
    }).join('');
}

// Configurar event listeners
function setupEventListeners() {
    // Búsqueda
    const searchInput = document.getElementById('searchInput');
    searchInput.addEventListener('input', Helpers.debounce((e) => {
        filterProductos();
    }, 300));
    
    // Filtros
    document.getElementById('filterCategoria').addEventListener('change', filterProductos);
    document.getElementById('filterEstado').addEventListener('change', filterProductos);
    
    // Formulario
    document.getElementById('productoForm').addEventListener('submit', handleSubmit);
}

// Filtrar productos
function filterProductos() {
    const searchTerm = document.getElementById('searchInput').value.toLowerCase();
    const categoriaFilter = document.getElementById('filterCategoria').value;
    const estadoFilter = document.getElementById('filterEstado').value;
    
    let filtered = productos;
    
    // Filtrar por búsqueda
    if (searchTerm) {
        filtered = filtered.filter(p => 
            p.nombre.toLowerCase().includes(searchTerm) ||
            p.codigoProducto.toLowerCase().includes(searchTerm) ||
            p.codigoBarras.toLowerCase().includes(searchTerm)
        );
    }
    
    // Filtrar por categoría
    if (categoriaFilter) {
        filtered = filtered.filter(p => p.categoriaId === parseInt(categoriaFilter));
    }
    
    // Filtrar por estado
    if (estadoFilter) {
        filtered = filtered.filter(p => p.activo === (estadoFilter === 'true'));
    }
    
    renderProductos(filtered);
}

// Abrir modal para crear
function openCreateModal() {
    currentProductoId = null;
    document.getElementById('modalTitle').textContent = 'Nuevo Producto';
    document.getElementById('productoForm').reset();
    document.getElementById('activo').checked = true;
}

// Abrir modal para editar
async function openEditModal(id) {
    try {
        currentProductoId = id;
        document.getElementById('modalTitle').textContent = 'Editar Producto';
        
        const producto = await productoService.getById(id);
        
        // Llenar formulario
        document.getElementById('productoId').value = producto.productoId;
        document.getElementById('codigoProducto').value = producto.codigoProducto;
        document.getElementById('codigoBarras').value = producto.codigoBarras;
        document.getElementById('nombre').value = producto.nombre;
        document.getElementById('descripcion').value = producto.descripcion || '';
        document.getElementById('precioUnitario').value = producto.precioUnitario;
        document.getElementById('categoriaId').value = producto.categoriaId;
        document.getElementById('codigoIva').value = producto.codigoIva;
        document.getElementById('stockActual').value = producto.stockActual;
        document.getElementById('stockMinimo').value = producto.stockMinimo;
        document.getElementById('stockMaximo').value = producto.stockMaximo;
        document.getElementById('activo').checked = producto.activo;
        
        // Mostrar modal
        const modal = new bootstrap.Modal(document.getElementById('productoModal'));
        modal.show();
    } catch (error) {
        console.error('Error al cargar producto:', error);
        Helpers.showToast('Error al cargar producto', 'error');
    }
}

// Manejar submit del formulario
async function handleSubmit(e) {
    e.preventDefault();
    
    // Validar formulario
    const validation = Validators.validateForm(e.target);
    if (!validation.isValid) {
        Validators.showFormErrors(e.target, validation.errors);
        return;
    }
    
    // Obtener datos del formulario
    const formData = new FormData(e.target);
    const producto = {
        codigoProducto: formData.get('codigoProducto'),
        codigoBarras: formData.get('codigoBarras'),
        nombre: formData.get('nombre'),
        descripcion: formData.get('descripcion'),
        precioUnitario: formData.get('precioUnitario'),
        categoriaId: formData.get('categoriaId'),
        codigoIva: formData.get('codigoIva'),
        stockActual: formData.get('stockActual'),
        stockMinimo: formData.get('stockMinimo'),
        stockMaximo: formData.get('stockMaximo'),
        activo: document.getElementById('activo').checked
    };
    
    try {
        Helpers.showLoading(true);
        
        if (currentProductoId) {
            // Actualizar
            await productoService.update(currentProductoId, producto);
            Helpers.showToast(AppConfig.MESSAGES.SUCCESS.UPDATE, 'success');
        } else {
            // Crear
            await productoService.create(producto);
            Helpers.showToast(AppConfig.MESSAGES.SUCCESS.SAVE, 'success');
        }
        
        // Cerrar modal y recargar
        const modal = bootstrap.Modal.getInstance(document.getElementById('productoModal'));
        modal.hide();
        await loadProductos();
        
    } catch (error) {
        console.error('Error al guardar producto:', error);
        Helpers.showToast(error.message || 'Error al guardar producto', 'error');
    } finally {
        Helpers.showLoading(false);
    }
}

// Eliminar producto
async function deleteProducto(id) {
    Helpers.showConfirmModal(
        'Confirmar eliminación',
        '¿Está seguro de que desea eliminar este producto?',
        async () => {
            try {
                Helpers.showLoading(true);
                await productoService.delete(id);
                Helpers.showToast(AppConfig.MESSAGES.SUCCESS.DELETE, 'success');
                await loadProductos();
            } catch (error) {
                console.error('Error al eliminar producto:', error);
                Helpers.showToast(error.message || 'Error al eliminar producto', 'error');
            } finally {
                Helpers.showLoading(false);
            }
        }
    );
}

// Inicializar al cargar la página
init();
