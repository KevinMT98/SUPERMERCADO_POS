/**
 * SUPERMERCADO POS - Gestión de Categorías de Productos
 */

// Variables globales
let categorias = [];
let categoriaActualId = null;
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
    configurarEventos();
});

/**
 * Cargar categorías
 */
async function cargarCategorias() {
    try {
        categorias = await categoriaService.getAll();
        inicializarDataTable();
    } catch (error) {
        console.error('Error al cargar categorías:', error);
        Helpers.showToast('Error al cargar categorías', 'error');
    }
}

/**
 * Inicializar DataTable
 */
function inicializarDataTable() {
    if (dataTable) {
        dataTable.destroy();
    }
    
    const data = categorias.map(categoria => {
        return [
            categoria.categoriaId,
            `<i class="bi bi-tag-fill text-primary category-icon"></i> ${categoria.descripcion}`,
            `<span class="badge ${categoria.activo ? 'bg-success' : 'bg-secondary'}">${categoria.activo ? 'Activo' : 'Inactivo'}</span>`,
            `<button class="btn btn-sm btn-primary" onclick="abrirModalEditar(${categoria.categoriaId})">
                <i class="bi bi-pencil"></i>
            </button>
            <button class="btn btn-sm btn-danger" onclick="eliminarCategoria(${categoria.categoriaId})">
                <i class="bi bi-trash"></i>
            </button>`
        ];
    });
    
    dataTable = $('#tablaCategorias').DataTable({
        data: data,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        },
        pageLength: 10,
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
        order: [[0, 'asc']],
        columnDefs: [
            { orderable: false, targets: 3 }
        ],
        responsive: true,
        drawCallback: function() {
            const $wrapper = $('#tablaCategorias').closest('.table-responsive');
            const currentHeight = $wrapper.height();
            $('#tablaCategorias tbody tr').css('opacity', '0');
            const newHeight = $wrapper[0].scrollHeight;
            if (Math.abs(currentHeight - newHeight) > 50) {
                $wrapper.css('height', currentHeight);
                $wrapper.animate({ height: newHeight }, 400, function() {
                    $wrapper.css('height', 'auto');
                });
            }
            $('#tablaCategorias tbody tr').each(function(index) {
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
    document.getElementById('filtroEstado').addEventListener('change', aplicarFiltros);
    
    // Formulario
    document.getElementById('formCategoria').addEventListener('submit', guardarCategoria);
}

/**
 * Aplicar filtros
 */
function aplicarFiltros() {
    const filtroEstado = document.getElementById('filtroEstado').value;
    
    // Aplicar filtro personalizado
    $.fn.dataTable.ext.search.push(
        function(settings, data, dataIndex) {
            // data[2] = Estado (HTML del badge)
            if (filtroEstado && !data[2].includes(filtroEstado)) {
                return false;
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
    document.getElementById('filtroEstado').value = '';
    dataTable.search('').columns().search('').draw();
}

/**
 * Abrir modal para nueva categoría
 */
function abrirModalNuevo() {
    categoriaActualId = null;
    document.getElementById('modalTitulo').textContent = 'Nueva Categoría';
    document.getElementById('formCategoria').reset();
    document.getElementById('activo').checked = true;
}

/**
 * Abrir modal para editar categoría
 */
async function abrirModalEditar(id) {
    try {
        categoriaActualId = id;
        document.getElementById('modalTitulo').textContent = 'Editar Categoría';
        
        const categoria = await categoriaService.getById(id);
        
        // Llenar formulario
        document.getElementById('categoriaId').value = categoria.categoriaId;
        document.getElementById('descripcion').value = categoria.descripcion;
        document.getElementById('activo').checked = categoria.activo;
        
        // Mostrar modal
        const modal = new bootstrap.Modal(document.getElementById('categoriaModal'));
        modal.show();
    } catch (error) {
        console.error('Error al cargar categoría:', error);
        Helpers.showToast('Error al cargar categoría', 'error');
    }
}

/**
 * Guardar categoría
 */
async function guardarCategoria(e) {
    e.preventDefault();
    
    const categoria = {
        Descripcion: document.getElementById('descripcion').value.trim(),
        Activo: document.getElementById('activo').checked
    };
    
    // Obtener el botón de submit
    const submitBtn = e.target.querySelector('button[type="submit"]');
    const originalBtnContent = submitBtn.innerHTML;
    
    try {
        // Deshabilitar botón
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';
        
        if (categoriaActualId) {
            // Actualizar
            await categoriaService.update(categoriaActualId, categoria);
            Helpers.showToast('Categoría actualizada exitosamente', 'success');
        } else {
            // Crear
            await categoriaService.create(categoria);
            Helpers.showToast('Categoría creada exitosamente', 'success');
        }
        
        // Cerrar modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('categoriaModal'));
        if (modal) {
            modal.hide();
        }
        
        // Pequeña pausa
        await new Promise(resolve => setTimeout(resolve, 300));
        
        // Recargar categorías
        await cargarCategorias();
        
        // Restaurar botón
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
        
    } catch (error) {
        console.error('Error al guardar categoría:', error);
        Helpers.showToast(error.message || 'Error al guardar categoría', 'error');
        
        // Restaurar botón
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
    }
}

/**
 * Eliminar categoría
 */
async function eliminarCategoria(id) {
    if (!confirm('¿Está seguro de eliminar esta categoría?\n\nNota: No se puede eliminar si tiene productos asociados.')) {
        return;
    }
    
    try {
        Helpers.showToast('Eliminando categoría...', 'info');
        
        await categoriaService.delete(id);
        Helpers.showToast('Categoría eliminada exitosamente', 'success');
        
        await cargarCategorias();
    } catch (error) {
        console.error('Error al eliminar categoría:', error);
        Helpers.showToast(error.message || 'Error al eliminar categoría', 'error');
    }
}
