/**
 * SUPERMERCADO POS - Gestión de Tarifas de IVA
 */

// Variables globales
let tarifas = [];
let tarifaActualId = null;
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
    await cargarTarifas();
    configurarEventos();
});

/**
 * Cargar tarifas
 */
async function cargarTarifas() {
    try {
        tarifas = await tarifaIvaService.getAll();
        inicializarDataTable();
    } catch (error) {
        console.error('Error al cargar tarifas:', error);
        Helpers.showToast('Error al cargar tarifas de IVA', 'error');
    }
}

/**
 * Inicializar DataTable
 */
function inicializarDataTable() {
    if (dataTable) {
        dataTable.destroy();
    }
    
    const data = tarifas.map(tarifa => {
        return [
            tarifa.tarifaIvaId,
            tarifa.codigoIva,
            tarifa.descripcion,
            `<span class="badge bg-info">${tarifa.porcentaje}%</span>`,
            `<span class="badge ${tarifa.estado ? 'bg-success' : 'bg-secondary'}">${tarifa.estado ? 'Activo' : 'Inactivo'}</span>`,
            `<button class="btn btn-sm btn-primary" onclick="abrirModalEditar(${tarifa.tarifaIvaId})">
                <i class="bi bi-pencil"></i>
            </button>
            <button class="btn btn-sm btn-danger" onclick="eliminarTarifa(${tarifa.tarifaIvaId})">
                <i class="bi bi-trash"></i>
            </button>`
        ];
    });
    
    dataTable = $('#tablaTarifas').DataTable({
        data: data,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        },
        pageLength: 10,
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
        order: [[0, 'asc']],
        columnDefs: [
            { orderable: false, targets: 5 }
        ],
        responsive: true,
        drawCallback: function() {
            const $wrapper = $('#tablaTarifas').closest('.table-responsive');
            const currentHeight = $wrapper.height();
            $('#tablaTarifas tbody tr').css('opacity', '0');
            const newHeight = $wrapper[0].scrollHeight;
            if (Math.abs(currentHeight - newHeight) > 50) {
                $wrapper.css('height', currentHeight);
                $wrapper.animate({ height: newHeight }, 400, function() {
                    $wrapper.css('height', 'auto');
                });
            }
            $('#tablaTarifas tbody tr').each(function(index) {
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
    document.getElementById('formTarifa').addEventListener('submit', guardarTarifa);
}

/**
 * Aplicar filtros
 */
function aplicarFiltros() {
    const filtroEstado = document.getElementById('filtroEstado').value;
    
    // Aplicar filtro personalizado
    $.fn.dataTable.ext.search.push(
        function(settings, data, dataIndex) {
            // data[4] = Estado (HTML del badge)
            if (filtroEstado && !data[4].includes(filtroEstado)) {
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
 * Abrir modal para nueva tarifa
 */
function abrirModalNuevo() {
    tarifaActualId = null;
    document.getElementById('modalTitulo').textContent = 'Nueva Tarifa de IVA';
    document.getElementById('formTarifa').reset();
    document.getElementById('activo').checked = true;
}

/**
 * Abrir modal para editar tarifa
 */
async function abrirModalEditar(id) {
    try {
        tarifaActualId = id;
        document.getElementById('modalTitulo').textContent = 'Editar Tarifa de IVA';
        
        const tarifa = await tarifaIvaService.getById(id);
        
        // Llenar formulario
        document.getElementById('tarifaId').value = tarifa.tarifaIvaId;
        document.getElementById('codigoIva').value = tarifa.codigoIva;
        document.getElementById('descripcion').value = tarifa.descripcion;
        document.getElementById('porcentaje').value = tarifa.porcentaje;
        document.getElementById('activo').checked = tarifa.estado;
        
        // Mostrar modal
        const modal = new bootstrap.Modal(document.getElementById('tarifaModal'));
        modal.show();
    } catch (error) {
        console.error('Error al cargar tarifa:', error);
        Helpers.showToast('Error al cargar tarifa', 'error');
    }
}

/**
 * Guardar tarifa
 */
async function guardarTarifa(e) {
    e.preventDefault();
    
    const tarifa = {
        CodigoIva: document.getElementById('codigoIva').value.trim(),
        Descripcion: document.getElementById('descripcion').value.trim(),
        Porcentaje: parseFloat(document.getElementById('porcentaje').value),
        Estado: document.getElementById('activo').checked
    };
    
    // Obtener el botón de submit
    const submitBtn = e.target.querySelector('button[type="submit"]');
    const originalBtnContent = submitBtn.innerHTML;
    
    try {
        // Deshabilitar botón
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';
        
        if (tarifaActualId) {
            // Actualizar
            await tarifaIvaService.update(tarifaActualId, tarifa);
            Helpers.showToast('Tarifa actualizada exitosamente', 'success');
        } else {
            // Crear
            await tarifaIvaService.create(tarifa);
            Helpers.showToast('Tarifa creada exitosamente', 'success');
        }
        
        // Cerrar modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('tarifaModal'));
        if (modal) {
            modal.hide();
        }
        
        // Pequeña pausa
        await new Promise(resolve => setTimeout(resolve, 300));
        
        // Recargar tarifas
        await cargarTarifas();
        
        // Restaurar botón
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
        
    } catch (error) {
        console.error('Error al guardar tarifa:', error);
        Helpers.showToast(error.message || 'Error al guardar tarifa', 'error');
        
        // Restaurar botón
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
    }
}

/**
 * Eliminar tarifa
 */
async function eliminarTarifa(id) {
    if (!confirm('¿Está seguro de eliminar esta tarifa de IVA?')) {
        return;
    }
    
    try {
        Helpers.showToast('Eliminando tarifa...', 'info');
        
        await tarifaIvaService.delete(id);
        Helpers.showToast('Tarifa eliminada exitosamente', 'success');
        
        await cargarTarifas();
    } catch (error) {
        console.error('Error al eliminar tarifa:', error);
        Helpers.showToast(error.message || 'Error al eliminar tarifa', 'error');
    }
}
