/**
 * SUPERMERCADO POS - Configuración
 */

// Configuración del Backend
const API_CONFIG = {
    BASE_URL: 'http://localhost:5276/api',
    ENDPOINTS: {
        LOGIN: '/Auth/login',
        LOGOUT: '/Auth/Logout'
    }
};

// Claves para localStorage
const STORAGE_KEYS = {
    TOKEN: 'token',
    EMAIL: 'email',
    ROLE: 'role',
    EXPIRE_IN: 'expireIn',
    REMEMBER_ME: 'rememberMe'
};
