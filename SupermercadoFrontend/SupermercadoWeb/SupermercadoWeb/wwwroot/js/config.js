/**
 * SUPERMERCADO POS - Configuración Central
 * Contiene todas las configuraciones globales de la aplicación
 */

const AppConfig = {
    // URL base del API Backend
    API_BASE_URL: 'http://localhost:5276/api',
    
    // Tiempo de expiración del token (en milisegundos)
    TOKEN_EXPIRATION_TIME: 3600000, // 1 hora
    
    // Claves para localStorage
    STORAGE_KEYS: {
        TOKEN: 'supermercado_token',
        USER: 'supermercado_user',
        REMEMBER_ME: 'supermercado_remember'
    },
    
    // Configuración de paginación
    PAGINATION: {
        DEFAULT_PAGE_SIZE: 10,
        PAGE_SIZE_OPTIONS: [5, 10, 25, 50, 100]
    },
    
    // Mensajes de la aplicación
    MESSAGES: {
        SUCCESS: {
            SAVE: 'Registro guardado exitosamente',
            UPDATE: 'Registro actualizado exitosamente',
            DELETE: 'Registro eliminado exitosamente',
            LOGIN: 'Inicio de sesión exitoso'
        },
        ERROR: {
            GENERIC: 'Ha ocurrido un error. Por favor, intente nuevamente',
            NETWORK: 'Error de conexión. Verifique su conexión a internet',
            UNAUTHORIZED: 'No tiene permisos para realizar esta acción',
            SESSION_EXPIRED: 'Su sesión ha expirado. Por favor, inicie sesión nuevamente',
            VALIDATION: 'Por favor, complete todos los campos requeridos'
        }
    },
    
    // Configuración de validación
    VALIDATION: {
        MIN_PASSWORD_LENGTH: 6,
        MAX_TEXT_LENGTH: 200,
        EMAIL_REGEX: /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    }
};

// Hacer la configuración disponible globalmente
window.AppConfig = AppConfig;
