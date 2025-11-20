/**
 * SUPERMERCADO POS - Gestión de Terceros
 */

// Variables globales
let terceros = [];
let tiposIdentificacion = [];
let terceroActualId = null;
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
    await cargarTiposIdentificacion();
    await cargarTerceros();
    configurarEventos();
});

/**
 * Cargar tipos de identificación
 */
async function cargarTiposIdentificacion() {
    try {
        const response = await apiClient.get('/TiposIdentificacion');
        console.log('Tipos de identificación recibidos:', response);
        tiposIdentificacion = response.filter(tipo => tipo.activo);
        console.log('Tipos de identificación activos:', tiposIdentificacion);
        
        const selectTipo = document.getElementById('codigoIdent');
        selectTipo.innerHTML = '<option value="">Seleccione...</option>';
        
        tiposIdentificacion.forEach(tipo => {
            // El backend puede usar diferentes nombres de campo: ID, id, tipoIdentificacionId, etc.
            const id = tipo.ID || tipo.id || tipo.tipoIdentificacionId || tipo.codigo_ident;
            const tipoDoc = tipo.tipoDocumentoID || tipo.tipoDocumento || tipo.codigo || '';
            const desc = tipo.descripcion || '';
            
            console.log('Agregando tipo:', { id, tipoDoc, desc });
            selectTipo.innerHTML += `<option value="${id}">${tipoDoc} - ${desc}</option>`;
        });
        
        console.log('Select poblado con opciones:', selectTipo.innerHTML);
        console.log('Total de opciones:', selectTipo.options.length);
    } catch (error) {
        console.error('Error al cargar tipos de identificación:', error);
        Helpers.showToast('Error al cargar tipos de identificación', 'error');
    }
}

/**
 * Cargar terceros
 */
async function cargarTerceros() {
    try {
        const response = await apiClient.get('/Tercero');
        terceros = response;
        inicializarDataTable();
    } catch (error) {
        console.error('Error al cargar terceros:', error);
        Helpers.showToast('Error al cargar terceros', 'error');
    }
}

/**
 * Inicializar DataTable
 */
function inicializarDataTable() {
    if (dataTable) {
        dataTable.destroy();
    }
    
    const data = terceros.map(tercero => {
        // Buscar el tipo de identificación usando el mismo campo que usamos para poblar el select
        const tipoIdent = tiposIdentificacion.find(t => {
            const id = t.ID || t.id || t.tipoIdentificacionId || t.codigo_ident;
            return id === tercero.codigo_ident;
        });
        
        const nombreCompleto = `${tercero.nombre} ${tercero.nombre2 || ''} ${tercero.apellido1} ${tercero.apellido2 || ''}`.trim();
        
        // Determinar tipo de tercero
        let tipoTercero = '';
        let badgeClass = '';
        if (tercero.es_cliente && tercero.es_proveedor) {
            tipoTercero = 'Cliente y Proveedor';
            badgeClass = 'badge-ambos';
        } else if (tercero.es_proveedor) {
            tipoTercero = 'Proveedor';
            badgeClass = 'badge-proveedor';
        } else if (tercero.es_cliente) {
            tipoTercero = 'Cliente';
            badgeClass = 'badge-cliente';
        }
        
        const tipoDocumento = tipoIdent ? (tipoIdent.tipoDocumentoID || tipoIdent.tipoDocumento || tipoIdent.codigo || 'N/A') : 'N/A';
        
        return [
            tercero.tercero_id,
            `<span class="badge bg-secondary">${tipoDocumento}</span><br><strong>${tercero.numero_identificacion}</strong>`,
            nombreCompleto,
            tercero.email,
            tercero.telefono || 'N/A',
            `<span class="badge ${badgeClass}">${tipoTercero}</span>`,
            `<span class="badge ${tercero.activo ? 'badge-activo' : 'badge-inactivo'}">${tercero.activo ? 'Activo' : 'Inactivo'}</span>`,
            `<button class="btn btn-sm btn-primary" onclick="abrirModalEditar(${tercero.tercero_id})">
                <i class="bi bi-pencil"></i>
            </button>
            <button class="btn btn-sm btn-danger" onclick="eliminarTercero(${tercero.tercero_id})">
                <i class="bi bi-trash"></i>
            </button>`
        ];
    });
    
    dataTable = $('#tablaTerceros').DataTable({
        data: data,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        },
        pageLength: 10,
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
        order: [[0, 'asc']],
        columnDefs: [
            { orderable: false, targets: 7 }
        ],
        responsive: true,
        drawCallback: function() {
            // Guardar altura actual antes del cambio
            const $wrapper = $('#tablaTerceros').closest('.table-responsive');
            const currentHeight = $wrapper.height();
            
            // Animación fade para las filas al redibujar
            $('#tablaTerceros tbody tr').css('opacity', '0');
            
            // Animar altura si cambió
            const newHeight = $wrapper[0].scrollHeight;
            if (Math.abs(currentHeight - newHeight) > 50) {
                $wrapper.css('height', currentHeight);
                $wrapper.animate({ height: newHeight }, 400, function() {
                    $wrapper.css('height', 'auto');
                });
            }
            
            // Fade in de filas
            $('#tablaTerceros tbody tr').each(function(index) {
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
    document.getElementById('filtroTipo').addEventListener('change', aplicarFiltros);
    document.getElementById('filtroEstado').addEventListener('change', aplicarFiltros);
    
    // Formulario
    document.getElementById('formTercero').addEventListener('submit', guardarTercero);
}

/**
 * Aplicar filtros
 */
function aplicarFiltros() {
    const tipoFiltro = document.getElementById('filtroTipo').value;
    const estadoFiltro = document.getElementById('filtroEstado').value;
    
    // Aplicar filtro personalizado
    $.fn.dataTable.ext.search.push(
        function(settings, data, dataIndex) {
            // data[5] = Tipo, data[6] = Estado (HTML del badge)
            
            // Filtrar por tipo
            if (tipoFiltro) {
                const tipoHtml = data[5];
                if (tipoFiltro === 'Cliente' && !tipoHtml.includes('Cliente') && !tipoHtml.includes('Cliente y Proveedor')) {
                    return false;
                }
                if (tipoFiltro === 'Proveedor' && !tipoHtml.includes('Proveedor')) {
                    return false;
                }
                if (tipoFiltro === 'Ambos' && !tipoHtml.includes('Cliente y Proveedor')) {
                    return false;
                }
            }
            
            // Filtrar por estado - buscar en el HTML del badge
            if (estadoFiltro) {
                const estadoHtml = data[6];
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
    document.getElementById('filtroTipo').value = '';
    document.getElementById('filtroEstado').value = '';
    dataTable.search('').columns().search('').draw();
}

/**
 * Abrir modal para nuevo tercero
 */
function abrirModalNuevo() {
    terceroActualId = null;
    document.getElementById('modalTitulo').textContent = 'Nuevo Tercero';
    const form = document.getElementById('formTercero');
    form.reset();
    form.classList.remove('was-validated');
    
    // Limpiar clases de validación
    form.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));
    form.querySelectorAll('.is-valid').forEach(el => el.classList.remove('is-valid'));
    
    document.getElementById('esCliente').checked = true;
    document.getElementById('esProveedor').checked = false;
    document.getElementById('activo').checked = true;
}

/**
 * Abrir modal para editar tercero
 */
async function abrirModalEditar(id) {
    try {
        terceroActualId = id;
        document.getElementById('modalTitulo').textContent = 'Editar Tercero';
        
        // Limpiar validaciones
        const form = document.getElementById('formTercero');
        form.classList.remove('was-validated');
        form.querySelectorAll('.is-invalid').forEach(el => el.classList.remove('is-invalid'));
        form.querySelectorAll('.is-valid').forEach(el => el.classList.remove('is-valid'));
        
        const tercero = await apiClient.get(`/Tercero/${id}`);
        
        // Llenar formulario
        document.getElementById('terceroId').value = tercero.tercero_id;
        document.getElementById('codigoIdent').value = tercero.codigo_ident;
        document.getElementById('numeroIdentificacion').value = tercero.numero_identificacion;
        document.getElementById('nombre').value = tercero.nombre;
        document.getElementById('nombre2').value = tercero.nombre2 || '';
        document.getElementById('apellido1').value = tercero.apellido1;
        document.getElementById('apellido2').value = tercero.apellido2 || '';
        document.getElementById('email').value = tercero.email;
        document.getElementById('telefono').value = tercero.telefono || '';
        document.getElementById('direccion').value = tercero.direccion || '';
        document.getElementById('esCliente').checked = tercero.es_cliente;
        document.getElementById('esProveedor').checked = tercero.es_proveedor;
        document.getElementById('activo').checked = tercero.activo;
        
        // Mostrar modal
        const modal = new bootstrap.Modal(document.getElementById('terceroModal'));
        modal.show();
    } catch (error) {
        console.error('Error al cargar tercero:', error);
        Helpers.showToast('Error al cargar tercero', 'error');
    }
}

/**
 * Guardar tercero
 */
async function guardarTercero(e) {
    e.preventDefault();
    
    // Validar que sea al menos cliente o proveedor
    const esCliente = document.getElementById('esCliente').checked;
    const esProveedor = document.getElementById('esProveedor').checked;
    
    if (!esCliente && !esProveedor) {
        Helpers.showToast('Debe seleccionar al menos Cliente o Proveedor', 'error');
        return;
    }
    
    // Validar formulario
    const form = document.getElementById('formTercero');
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return;
    }
    
    // Validar campos requeridos adicionales
    const codigoIdent = document.getElementById('codigoIdent').value;
    console.log('Código Ident seleccionado:', codigoIdent, typeof codigoIdent);
    
    if (!codigoIdent || codigoIdent === '') {
        Helpers.showToast('Debe seleccionar un tipo de identificación', 'error');
        document.getElementById('codigoIdent').classList.add('is-invalid');
        return;
    }
    
    const codigoIdentInt = parseInt(codigoIdent);
    if (isNaN(codigoIdentInt)) {
        Helpers.showToast('Tipo de identificación inválido', 'error');
        document.getElementById('codigoIdent').classList.add('is-invalid');
        return;
    }
    
    const tercero = {
        codigo_ident: codigoIdentInt,
        numero_identificacion: document.getElementById('numeroIdentificacion').value.trim(),
        nombre: document.getElementById('nombre').value.trim(),
        nombre2: document.getElementById('nombre2').value.trim() || null,
        apellido1: document.getElementById('apellido1').value.trim(),
        apellido2: document.getElementById('apellido2').value.trim() || null,
        email: document.getElementById('email').value.trim(),
        direccion: document.getElementById('direccion').value.trim() || '',
        telefono: document.getElementById('telefono').value.trim() || '',
        es_cliente: esCliente,
        es_proveedor: esProveedor,
        activo: document.getElementById('activo').checked
    };
    
    // Para actualización, agregar el ID
    if (terceroActualId) {
        tercero.tercero_id = terceroActualId;
    }
    
    console.log('Datos del tercero a enviar:', tercero);
    
    // Obtener el botón de submit
    const submitBtn = e.target.querySelector('button[type="submit"]');
    const originalBtnContent = submitBtn.innerHTML;
    
    try {
        // Deshabilitar botón de guardar para evitar doble click
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';
        
        if (terceroActualId) {
            // Actualizar
            console.log('Actualizando tercero con ID:', terceroActualId);
            await apiClient.put(`/Tercero/${terceroActualId}`, tercero);
            Helpers.showToast('Tercero actualizado exitosamente', 'success');
        } else {
            // Crear
            console.log('Creando nuevo tercero');
            console.log('JSON a enviar:', JSON.stringify(tercero, null, 2));
            await apiClient.post('/Tercero', tercero);
            Helpers.showToast('Tercero creado exitosamente', 'success');
        }
        
        // Cerrar modal primero
        const modal = bootstrap.Modal.getInstance(document.getElementById('terceroModal'));
        if (modal) {
            modal.hide();
        }
        
        // Pequeña pausa para que el modal se cierre visualmente
        await new Promise(resolve => setTimeout(resolve, 300));
        
        // Recargar terceros en segundo plano
        await cargarTerceros();
        
        // Restaurar botón después de completar
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
        
    } catch (error) {
        console.error('Error al guardar tercero:', error);
        Helpers.showToast(error.message || 'Error al guardar tercero', 'error');
        
        // Restaurar botón en caso de error
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
    }
}

/**
 * Eliminar tercero
 */
async function eliminarTercero(id) {
    if (!confirm('¿Está seguro de eliminar este tercero?')) {
        return;
    }
    
    try {
        // Mostrar toast de procesando
        Helpers.showToast('Eliminando tercero...', 'info');
        
        await apiClient.delete(`/Tercero/${id}`);
        Helpers.showToast('Tercero eliminado exitosamente', 'success');
        
        // Recargar terceros en segundo plano
        await cargarTerceros();
    } catch (error) {
        console.error('Error al eliminar tercero:', error);
        Helpers.showToast(error.message || 'Error al eliminar tercero', 'error');
    }
}
