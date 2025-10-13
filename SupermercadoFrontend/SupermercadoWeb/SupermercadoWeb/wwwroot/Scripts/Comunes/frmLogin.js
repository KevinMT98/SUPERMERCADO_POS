/**
 * ⚠️ ARCHIVO OBSOLETO - NO USAR ⚠️
 * 
 * Este archivo ha sido reemplazado por el sistema de autenticación modular en:
 * - js/services/authService.js
 * - js/utils/storage.js
 * - js/api/apiClient.js
 * 
 * El login.html ahora usa estos servicios directamente.
 * Este archivo se mantiene temporalmente para referencia pero NO DEBE SER USADO.
 * 
 * SUPERMERCADO POS - Login Form (DEPRECADO)
 * Manejo de autenticación con JWT
 */

// Configuración de la API
const API_BASE_URL = 'http://localhost:5276/api'; // Backend en puerto 5276

// Verificar si ya está autenticado
if (localStorage.getItem('token')) {
    window.location.href = '/dashboard.html';
}

// Manejar el formulario de login
document.getElementById('loginForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    
    const email = document.getElementById('email').value.trim();
    const password = document.getElementById('password').value;
    const remember = document.getElementById('remember').checked;
    
    // Validar campos
    if (!email || !password) {
        mostrarAlerta('Por favor, complete todos los campos', 'warning');
        return;
    }
    
    // Validar formato de email
    if (!validarEmail(email)) {
        mostrarAlerta('Por favor, ingrese un email válido', 'warning');
        return;
    }
    
    // Mostrar loading
    const btnText = document.getElementById('btnText');
    const btnSpinner = document.getElementById('btnSpinner');
    const submitBtn = document.querySelector('button[type="submit"]');
    
    btnText.classList.add('d-none');
    btnSpinner.classList.remove('d-none');
    submitBtn.disabled = true;
    
    try {
        // Llamar al endpoint de login
        const response = await fetch(`${API_BASE_URL}/Auth/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                email: email,
                password: password
            })
        });
        
        if (!response.ok) {
            // Manejar errores HTTP
            const errorData = await response.json();
            throw new Error(errorData.message || 'Credenciales inválidas');
        }
        
        // Obtener el token
        const data = await response.json();
        
        // Guardar token y datos del usuario en localStorage
        localStorage.setItem('token', data.accesToken);
        localStorage.setItem('email', data.email);
        localStorage.setItem('role', data.role);
        localStorage.setItem('expireIn', data.expireIn);
        
        if (remember) {
            localStorage.setItem('rememberMe', 'true');
        }
        
        // Mostrar mensaje de éxito
        mostrarAlerta('Inicio de sesión exitoso', 'success');
        
        // Redirigir al dashboard después de 1 segundo
        setTimeout(() => {
            window.location.href = '/dashboard.html';
        }, 1000);
        
    } catch (error) {
        console.error('Error en login:', error);
        mostrarAlerta(error.message || 'Error al iniciar sesión. Verifique sus credenciales.', 'danger');
        
        // Restaurar botón
        btnText.classList.remove('d-none');
        btnSpinner.classList.add('d-none');
        submitBtn.disabled = false;
    }
});

/**
 * Valida formato de email
 */
function validarEmail(email) {
    const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return regex.test(email);
}

/**
 * Muestra una alerta temporal
 */
function mostrarAlerta(mensaje, tipo) {
    // Crear contenedor de alertas si no existe
    let alertContainer = document.getElementById('alert-container');
    if (!alertContainer) {
        alertContainer = document.createElement('div');
        alertContainer.id = 'alert-container';
        alertContainer.style.position = 'fixed';
        alertContainer.style.top = '20px';
        alertContainer.style.right = '20px';
        alertContainer.style.zIndex = '9999';
        alertContainer.style.maxWidth = '400px';
        document.body.appendChild(alertContainer);
    }
    
    // Crear alerta
    const alertId = 'alert-' + Date.now();
    const alertHTML = `
        <div id="${alertId}" class="alert alert-${tipo} alert-dismissible fade show" role="alert">
            ${mensaje}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;
    
    alertContainer.insertAdjacentHTML('beforeend', alertHTML);
    
    // Auto-eliminar después de 5 segundos
    setTimeout(() => {
        const alertElement = document.getElementById(alertId);
        if (alertElement) {
            alertElement.remove();
        }
    }, 5000);
}
