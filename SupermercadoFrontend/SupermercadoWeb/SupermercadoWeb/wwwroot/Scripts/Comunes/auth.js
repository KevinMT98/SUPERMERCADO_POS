/**
 * SUPERMERCADO POS - Auth Helper
 * Funciones para verificar autenticación y proteger páginas
 */

/**
 * Verifica si el usuario está autenticado
 * @returns {boolean}
 */
function isAuthenticated() {
    const token = localStorage.getItem('token');
    return token !== null && token !== '';
}

/**
 * Obtiene el token JWT
 * @returns {string|null}
 */
function getToken() {
    return localStorage.getItem('token');
}

/**
 * Obtiene los datos del usuario
 * @returns {object|null}
 */
function getUserData() {
    if (!isAuthenticated()) {
        return null;
    }
    
    return {
        email: localStorage.getItem('email'),
        role: localStorage.getItem('role'),
        expireIn: localStorage.getItem('expireIn')
    };
}

/**
 * Protege una página - redirige a login si no está autenticado
 */
function requireAuth() {
    if (!isAuthenticated()) {
        window.location.href = '/login.html';
    }
}

/**
 * Cierra sesión
 */
function logout() {
    // Limpiar localStorage
    localStorage.removeItem('token');
    localStorage.removeItem('email');
    localStorage.removeItem('role');
    localStorage.removeItem('expireIn');
    localStorage.removeItem('rememberMe');
    
    // Redirigir a login
    window.location.href = '/login.html';
}

/**
 * Obtiene headers con autorización para peticiones HTTP
 * @returns {object}
 */
function getAuthHeaders() {
    const token = getToken();
    return {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };
}
