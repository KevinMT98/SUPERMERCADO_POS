/**
 * SUPERMERCADO POS - Gestión de Roles
 */

// Variables globales
let roles = [];
let rolActualId = null;
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
    await cargarRoles();
    configurarEventos();
});

/**
 * Cargar roles
 */
async function cargarRoles() {
    try {
        roles = await rolService.getAll();
        inicializarDataTable();
    } catch (error) {
        console.error('Error al cargar roles:', error);
        Helpers.showToast('Error al cargar roles', 'error');
    }
}

/**
 * Inicializar DataTable
 */
function inicializarDataTable() {
    if (dataTable) {
        dataTable.destroy();
    }
    
    const data = roles.map(rol => {
        return [
            rol.rol_id,
            `<i class="bi bi-shield-check role-icon text-primary"></i> ${rol.nombre}`,
            `<span class="badge ${rol.activo ? 'bg-success' : 'bg-secondary'}">${rol.activo ? 'Activo' : 'Inactivo'}</span>`,
            `<button class="btn btn-sm btn-primary" onclick="abrirModalEditar(${rol.rol_id})">
                <i class="bi bi-pencil"></i>
            </button>
            <button class="btn btn-sm btn-danger" onclick="eliminarRol(${rol.rol_id})">
                <i class="bi bi-trash"></i>
            </button>`
        ];
    });
    
    dataTable = $('#tablaRoles').DataTable({
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
            const $wrapper = $('#tablaRoles').closest('.table-responsive');
            const currentHeight = $wrapper.height();
            $('#tablaRoles tbody tr').css('opacity', '0');
            const newHeight = $wrapper[0].scrollHeight;
            if (Math.abs(currentHeight - newHeight) > 50) {
                $wrapper.css('height', currentHeight);
                $wrapper.animate({ height: newHeight }, 400, function() {
                    $wrapper.css('height', 'auto');
                });
            }
            $('#tablaRoles tbody tr').each(function(index) {
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
    document.getElementById('formRol').addEventListener('submit', guardarRol);
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
 * Abrir modal para nuevo rol
 */
function abrirModalNuevo() {
    rolActualId = null;
    document.getElementById('modalTitulo').textContent = 'Nuevo Rol';
    document.getElementById('formRol').reset();
    document.getElementById('activo').checked = true;
}

/**
 * Abrir modal para editar rol
 */
async function abrirModalEditar(id) {
    try {
        rolActualId = id;
        document.getElementById('modalTitulo').textContent = 'Editar Rol';
        
        const rol = await rolService.getById(id);
        
        // Llenar formulario
        document.getElementById('rolId').value = rol.rol_id;
        document.getElementById('nombre').value = rol.nombre;
        document.getElementById('activo').checked = rol.activo;
        
        // Mostrar modal
        const modal = new bootstrap.Modal(document.getElementById('rolModal'));
        modal.show();
    } catch (error) {
        console.error('Error al cargar rol:', error);
        Helpers.showToast('Error al cargar rol', 'error');
    }
}

/**
 * Guardar rol
 */
async function guardarRol(e) {
    e.preventDefault();
    
    const rol = {
        nombre: document.getElementById('nombre').value.trim(),
        activo: document.getElementById('activo').checked
    };
    
    // Obtener el botón de submit
    const submitBtn = e.target.querySelector('button[type="submit"]');
    const originalBtnContent = submitBtn.innerHTML;
    
    try {
        // Deshabilitar botón
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';
        
        if (rolActualId) {
            // Actualizar
            await rolService.update(rolActualId, rol);
            Helpers.showToast('Rol actualizado exitosamente', 'success');
        } else {
            // Crear
            await rolService.create(rol);
            Helpers.showToast('Rol creado exitosamente', 'success');
        }
        
        // Cerrar modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('rolModal'));
        modal.hide();
        
        // Recargar tabla
        await cargarRoles();
    } catch (error) {
        console.error('Error al guardar rol:', error);
        Helpers.showToast(error.message || 'Error al guardar rol', 'error');
    } finally {
        // Restaurar botón
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
    }
}

/**
 * Eliminar rol
 */
async function eliminarRol(id) {
    if (!confirm('¿Está seguro de eliminar este rol?')) {
        return;
    }
    
    try {
        Helpers.showToast('Eliminando rol...', 'info');
        
        await rolService.delete(id);
        
        Helpers.showToast('Rol eliminado exitosamente', 'success');
        await cargarRoles();
    } catch (error) {
        console.error('Error al eliminar rol:', error);
        Helpers.showToast(error.message || 'Error al eliminar rol', 'error');
    }
}
