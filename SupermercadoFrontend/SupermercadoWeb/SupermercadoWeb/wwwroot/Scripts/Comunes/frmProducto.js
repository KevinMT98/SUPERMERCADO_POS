/**
 * SUPERMERCADO POS - Gestión de Productos
 */

// Variables globales
let productos = [];
let categorias = [];
let tarifasIva = [];
let productoActualId = null;
let dataTable = null;

// Proteger la página
authService.requireAuth();

// Mostrar email del usuario
const user = authService.getCurrentUser();
if (user) {
    document.getElementById('userEmail').textContent = user.email;
}

// Inicializar
document.addEventListener('DOMContentLoaded', async () => {
    await cargarCategorias();
    await cargarTarifasIva();
    await cargarProductos();
    configurarEventos();
});

/**
 * Cargar categorías
 */
async function cargarCategorias() {
    try {
        // Cargar todas las categorías (para buscar en la tabla)
        categorias = await categoriaService.getAll();
        
        // Cargar solo categorías activas
        const categoriasActivas = await categoriaService.getAllActivos();
        
        // Llenar select del modal (solo activas)
        const selectCategoria = document.getElementById('categoriaId');
        selectCategoria.innerHTML = '<option value="">Seleccione...</option>';
        categoriasActivas.forEach(cat => {
            selectCategoria.innerHTML += `<option value="${cat.categoriaId}">${cat.descripcion}</option>`;
        });
        
        // Llenar filtro (solo activas) - usar descripción para filtrar
        const filtroCategoria = document.getElementById('filtroCategoria');
        filtroCategoria.innerHTML = '<option value="">Todas las categorías</option>';
        categoriasActivas.forEach(cat => {
            filtroCategoria.innerHTML += `<option value="${cat.descripcion}">${cat.descripcion}</option>`;
        });
    } catch (error) {
        console.error('Error al cargar categorías:', error);
        Helpers.showToast('Error al cargar categorías', 'error');
    }
}

/**
 * Cargar tarifas de IVA
 */
async function cargarTarifasIva() {
    try {
        // Cargar solo tarifas activas para el select del modal
        tarifasIva = await tarifaIvaService.getAllActivos();
        
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

/**
 * Cargar productos
 */
async function cargarProductos() {
    try {
        productos = await productoService.getAll();
        inicializarDataTable();
    } catch (error) {
        console.error('Error al cargar productos:', error);
        Helpers.showToast('Error al cargar productos', 'error');
    }
}

/**
 * Inicializar DataTable
 */
function inicializarDataTable() {
    if (dataTable) {
        dataTable.destroy();
    }
    
    const data = productos.map(producto => {
        const categoria = categorias.find(c => c.categoriaId === producto.categoriaId);
        const iva = tarifasIva.find(i => i.tarifaIvaId === producto.codigoIva);
        const stockClass = producto.stockActual < producto.stockMinimo ? 'badge-stock-bajo' : 'badge-stock-ok';
        
        return [
            producto.codigoProducto,
            producto.codigoBarras,
            producto.nombre,
            categoria ? categoria.descripcion : 'N/A',
            `$${producto.precioUnitario.toLocaleString('es-CO')}`,
            `<span class="badge ${stockClass}">${producto.stockActual} / ${producto.stockMinimo}</span>`,
            iva ? iva.porcentaje + '%' : 'N/A',
            `<span class="badge ${producto.activo ? 'bg-success' : 'bg-secondary'}">${producto.activo ? 'Activo' : 'Inactivo'}</span>`,
            `<button class="btn btn-sm btn-primary" onclick="abrirModalEditar(${producto.productoId})">
                <i class="bi bi-pencil"></i>
            </button>
            <button class="btn btn-sm btn-danger" onclick="eliminarProducto(${producto.productoId})">
                <i class="bi bi-trash"></i>
            </button>`
        ];
    });
    
    dataTable = $('#tablaProductos').DataTable({
        data: data,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        },
        pageLength: 10,
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
        order: [[0, 'asc']],
        columnDefs: [
            { orderable: false, targets: 8 }
        ],
        responsive: true,
        drawCallback: function() {
            // Guardar altura actual antes del cambio
            const $wrapper = $('#tablaProductos').closest('.table-responsive');
            const currentHeight = $wrapper.height();
            
            // Animación fade para las filas al redibujar
            $('#tablaProductos tbody tr').css('opacity', '0');
            
            // Animar altura si cambió
            const newHeight = $wrapper[0].scrollHeight;
            if (Math.abs(currentHeight - newHeight) > 50) {
                $wrapper.css('height', currentHeight);
                $wrapper.animate({ height: newHeight }, 400, function() {
                    $wrapper.css('height', 'auto');
                });
            }
            
            // Fade in de filas
            $('#tablaProductos tbody tr').each(function(index) {
                $(this).delay(index * 30).animate({ opacity: 1 }, 300);
            });
        }
    });
}

/**
 * Configurar eventos
 */
function configurarEventos() {
    // Filtros
    document.getElementById('filtroCategoria').addEventListener('change', aplicarFiltros);
    document.getElementById('filtroEstado').addEventListener('change', aplicarFiltros);
    
    // Formulario
    document.getElementById('formProducto').addEventListener('submit', guardarProducto);
}

/**
 * Aplicar filtros
 */
function aplicarFiltros() {
    const categoriaFiltro = document.getElementById('filtroCategoria').value;
    const estadoFiltro = document.getElementById('filtroEstado').value;
    
    // Aplicar filtro personalizado
    $.fn.dataTable.ext.search.push(
        function(settings, data, dataIndex) {
            // data[3] = Categoría, data[7] = Estado (HTML del badge)
            
            // Filtrar por categoría
            if (categoriaFiltro && data[3] !== categoriaFiltro) {
                return false;
            }
            
            // Filtrar por estado - buscar en el HTML del badge
            if (estadoFiltro) {
                const estadoHtml = data[7];
                if (!estadoHtml.includes(estadoFiltro)) {
                    return false;
                }
            }
            
            return true;
        }
    );
    
    // Redibujar tabla
    dataTable.draw();
    
    // Limpiar filtro personalizado después de dibujar
    $.fn.dataTable.ext.search.pop();
}

/**
 * Limpiar filtros
 */
function limpiarFiltros() {
    document.getElementById('filtroCategoria').value = '';
    document.getElementById('filtroEstado').value = '';
    dataTable.search('').columns().search('').draw();
}

/**
 * Abrir modal para nuevo producto
 */
function abrirModalNuevo() {
    productoActualId = null;
    document.getElementById('modalTitulo').textContent = 'Nuevo Producto';
    document.getElementById('formProducto').reset();
    document.getElementById('activo').checked = true;
}

/**
 * Abrir modal para editar producto
 */
async function abrirModalEditar(id) {
    try {
        productoActualId = id;
        document.getElementById('modalTitulo').textContent = 'Editar Producto';
        
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

/**
 * Guardar producto
 */
async function guardarProducto(e) {
    e.preventDefault();
    
    const producto = {
        codigoProducto: document.getElementById('codigoProducto').value,
        codigoBarras: document.getElementById('codigoBarras').value,
        nombre: document.getElementById('nombre').value,
        descripcion: document.getElementById('descripcion').value,
        precioUnitario: parseFloat(document.getElementById('precioUnitario').value),
        categoriaId: parseInt(document.getElementById('categoriaId').value),
        codigoIva: parseInt(document.getElementById('codigoIva').value),
        stockActual: parseInt(document.getElementById('stockActual').value),
        stockMinimo: parseInt(document.getElementById('stockMinimo').value),
        stockMaximo: parseInt(document.getElementById('stockMaximo').value),
        activo: document.getElementById('activo').checked
    };
    
    // Obtener el botón de submit
    const submitBtn = e.target.querySelector('button[type="submit"]');
    const originalBtnContent = submitBtn.innerHTML;
    
    try {
        // Deshabilitar botón de guardar para evitar doble click
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';
        
        if (productoActualId) {
            // Actualizar
            await productoService.update(productoActualId, producto);
            Helpers.showToast('Producto actualizado exitosamente', 'success');
        } else {
            // Crear
            await productoService.create(producto);
            Helpers.showToast('Producto creado exitosamente', 'success');
        }
        
        // Cerrar modal primero
        const modal = bootstrap.Modal.getInstance(document.getElementById('productoModal'));
        if (modal) {
            modal.hide();
        }
        
        // Pequeña pausa para que el modal se cierre visualmente
        await new Promise(resolve => setTimeout(resolve, 300));
        
        // Recargar productos en segundo plano
        await cargarProductos();
        
        // Restaurar botón después de completar
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
        
    } catch (error) {
        console.error('Error al guardar producto:', error);
        Helpers.showToast(error.message || 'Error al guardar producto', 'error');
        
        // Restaurar botón en caso de error
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
    }
}

/**
 * Eliminar producto
 */
async function eliminarProducto(id) {
    if (!confirm('¿Está seguro de eliminar este producto?')) {
        return;
    }
    
    try {
        // Mostrar toast de procesando
        Helpers.showToast('Eliminando producto...', 'info');
        
        await productoService.delete(id);
        Helpers.showToast('Producto eliminado exitosamente', 'success');
        
        // Recargar productos en segundo plano
        await cargarProductos();
    } catch (error) {
        console.error('Error al eliminar producto:', error);
        Helpers.showToast(error.message || 'Error al eliminar producto', 'error');
    }
}
