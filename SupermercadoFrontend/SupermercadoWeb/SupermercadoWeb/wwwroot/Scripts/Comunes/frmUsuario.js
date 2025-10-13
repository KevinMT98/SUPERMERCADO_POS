/**
 * SUPERMERCADO POS - Gestión de Usuarios
 */

// Variables globales
let usuarios = [];
let roles = [];
let usuarioActualId = null;
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
    await cargarUsuarios();
    configurarEventos();
});

/**
 * Cargar roles
 */
async function cargarRoles() {
    try {
        // Cargar todos los roles
        roles = await rolService.getAll();
        
        // Cargar solo roles activos para el select del modal
        const rolesActivos = await rolService.getAllActivos();
        
        // Llenar select del modal (solo activos)
        const selectRol = document.getElementById('rol');
        selectRol.innerHTML = '<option value="">Seleccione...</option>';
        rolesActivos.forEach(rol => {
            selectRol.innerHTML += `<option value="${rol.rol_id}">${rol.nombre}</option>`;
        });
        
        // Llenar filtro (todos los roles)
        const filtroRol = document.getElementById('filtroRol');
        filtroRol.innerHTML = '<option value="">Todos los roles</option>';
        roles.forEach(rol => {
            filtroRol.innerHTML += `<option value="${rol.nombre}">${rol.nombre}</option>`;
        });
    } catch (error) {
        console.error('Error al cargar roles:', error);
        Helpers.showToast('Error al cargar roles', 'error');
    }
}

/**
 * Cargar usuarios
 */
async function cargarUsuarios() {
    try {
        usuarios = await usuarioService.getAll();
        inicializarDataTable();
    } catch (error) {
        console.error('Error al cargar usuarios:', error);
        Helpers.showToast('Error al cargar usuarios', 'error');
    }
}

/**
 * Inicializar DataTable
 */
function inicializarDataTable() {
    // Destruir DataTable existente si existe
    if (dataTable) {
        dataTable.destroy();
    }
    
    // Preparar datos para DataTable
    const data = usuarios.map(usuario => {
        const rol = roles.find(r => r.rol_id === usuario.rol);
        const rolNombre = rol ? rol.nombre : 'Sin rol';
        
        return [
            usuario.usuarioId,
            `<i class="bi bi-person-circle user-icon text-primary"></i> ${usuario.nombreUsuario}`,
            `${usuario.nombre} ${usuario.apellido}`,
            usuario.email,
            `<span class="badge bg-info">${rolNombre}</span>`,
            `<span class="badge ${usuario.activo ? 'bg-success' : 'bg-secondary'}">${usuario.activo ? 'Activo' : 'Inactivo'}</span>`,
            `<button class="btn btn-sm btn-primary" onclick="abrirModalEditar(${usuario.usuarioId})">
                <i class="bi bi-pencil"></i>
            </button>
            <button class="btn btn-sm btn-danger" onclick="eliminarUsuario(${usuario.usuarioId})">
                <i class="bi bi-trash"></i>
            </button>`
        ];
    });
    
    // Inicializar DataTable
    dataTable = $('#tablaUsuarios').DataTable({
        data: data,
        language: {
            url: '//cdn.datatables.net/plug-ins/1.13.6/i18n/es-ES.json'
        },
        pageLength: 10,
        lengthMenu: [[5, 10, 25, 50, -1], [5, 10, 25, 50, "Todos"]],
        order: [[0, 'asc']],
        columnDefs: [
            { orderable: false, targets: 6 } // Deshabilitar ordenamiento en columna de acciones
        ],
        responsive: true,
        dom: '<"row"<"col-sm-12 col-md-6"l><"col-sm-12 col-md-6"f>>rtip',
        drawCallback: function() {
            // Guardar altura actual antes del cambio
            const $wrapper = $('#tablaUsuarios').closest('.table-responsive');
            const currentHeight = $wrapper.height();
            
            // Animación fade para las filas al redibujar
            $('#tablaUsuarios tbody tr').css('opacity', '0');
            
            // Animar altura si cambió
            const newHeight = $wrapper[0].scrollHeight;
            if (Math.abs(currentHeight - newHeight) > 50) {
                $wrapper.css('height', currentHeight);
                $wrapper.animate({ height: newHeight }, 400, function() {
                    $wrapper.css('height', 'auto');
                });
            }
            
            // Fade in de filas
            $('#tablaUsuarios tbody tr').each(function(index) {
                $(this).delay(index * 30).animate({ opacity: 1 }, 300);
            });
        }
    });
}

/**
 * Configurar eventos
 */
function configurarEventos() {
    // Formulario
    document.getElementById('formUsuario').addEventListener('submit', guardarUsuario);
    
    // Filtros personalizados
    document.getElementById('filtroRol').addEventListener('change', aplicarFiltros);
    document.getElementById('filtroEstado').addEventListener('change', aplicarFiltros);
}

/**
 * Aplicar filtros personalizados a DataTable
 */
function aplicarFiltros() {
    const filtroRol = document.getElementById('filtroRol').value;
    const filtroEstado = document.getElementById('filtroEstado').value;
    
    // Aplicar filtro personalizado
    $.fn.dataTable.ext.search.push(
        function(settings, data, dataIndex) {
            // data[4] = Rol (HTML del badge), data[5] = Estado (HTML del badge)
            
            // Filtrar por rol
            if (filtroRol && !data[4].includes(filtroRol)) {
                return false;
            }
            
            // Filtrar por estado
            if (filtroEstado && !data[5].includes(filtroEstado)) {
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
    document.getElementById('filtroRol').value = '';
    document.getElementById('filtroEstado').value = '';
    
    // Limpiar búsqueda de DataTable
    dataTable.search('').columns().search('').draw();
}

/**
 * Abrir modal para nuevo usuario
 */
function abrirModalNuevo() {
    usuarioActualId = null;
    document.getElementById('modalTitulo').textContent = 'Nuevo Usuario';
    document.getElementById('formUsuario').reset();
    document.getElementById('activo').checked = true;
    
    // Hacer contraseña obligatoria
    document.getElementById('password').required = true;
    document.getElementById('passwordLabel').textContent = '*';
    document.getElementById('passwordHelp').textContent = 'Mínimo 6 caracteres';
}

/**
 * Abrir modal para editar usuario
 */
async function abrirModalEditar(id) {
    try {
        usuarioActualId = id;
        document.getElementById('modalTitulo').textContent = 'Editar Usuario';
        
        const usuario = await usuarioService.getById(id);
        
        // Llenar formulario
        document.getElementById('usuarioId').value = usuario.usuarioId;
        document.getElementById('nombreUsuario').value = usuario.nombreUsuario;
        document.getElementById('nombre').value = usuario.nombre;
        document.getElementById('apellido').value = usuario.apellido;
        document.getElementById('email').value = usuario.email;
        document.getElementById('rol').value = usuario.rol;
        document.getElementById('activo').checked = usuario.activo;
        
        // Limpiar contraseña y hacerla opcional
        document.getElementById('password').value = '';
        document.getElementById('password').required = false;
        document.getElementById('passwordLabel').textContent = '';
        document.getElementById('passwordHelp').textContent = 'Dejar en blanco para mantener la actual';
        
        // Mostrar modal
        const modal = new bootstrap.Modal(document.getElementById('usuarioModal'));
        modal.show();
    } catch (error) {
        console.error('Error al cargar usuario:', error);
        Helpers.showToast('Error al cargar usuario', 'error');
    }
}

/**
 * Guardar usuario
 */
async function guardarUsuario(e) {
    e.preventDefault();
    
    const usuario = {
        nombreUsuario: document.getElementById('nombreUsuario').value.trim(),
        nombre: document.getElementById('nombre').value.trim(),
        apellido: document.getElementById('apellido').value.trim(),
        email: document.getElementById('email').value.trim(),
        rol: parseInt(document.getElementById('rol').value),
        activo: document.getElementById('activo').checked
    };
    
    // Agregar contraseña solo si se proporcionó
    const password = document.getElementById('password').value;
    if (password) {
        usuario.password = password;
    }
    
    // Obtener el botón de submit
    const submitBtn = e.target.querySelector('button[type="submit"]');
    const originalBtnContent = submitBtn.innerHTML;
    
    try {
        // Deshabilitar botón
        submitBtn.disabled = true;
        submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';
        
        if (usuarioActualId) {
            // Actualizar
            await usuarioService.update(usuarioActualId, usuario);
            Helpers.showToast('Usuario actualizado exitosamente', 'success');
        } else {
            // Crear - la contraseña es obligatoria
            if (!password) {
                throw new Error('La contraseña es obligatoria para crear un usuario');
            }
            await usuarioService.create(usuario);
            Helpers.showToast('Usuario creado exitosamente', 'success');
        }
        
        // Cerrar modal
        const modal = bootstrap.Modal.getInstance(document.getElementById('usuarioModal'));
        modal.hide();
        
        // Recargar tabla
        await cargarUsuarios();
    } catch (error) {
        console.error('Error al guardar usuario:', error);
        Helpers.showToast(error.message || 'Error al guardar usuario', 'error');
    } finally {
        // Restaurar botón
        submitBtn.disabled = false;
        submitBtn.innerHTML = originalBtnContent;
    }
}

/**
 * Eliminar usuario
 */
async function eliminarUsuario(id) {
    if (!confirm('¿Está seguro de eliminar este usuario?')) {
        return;
    }
    
    try {
        Helpers.showToast('Eliminando usuario...', 'info');
        
        await usuarioService.delete(id);
        
        Helpers.showToast('Usuario eliminado exitosamente', 'success');
        await cargarUsuarios();
    } catch (error) {
        console.error('Error al eliminar usuario:', error);
        Helpers.showToast(error.message || 'Error al eliminar usuario', 'error');
    }
}
