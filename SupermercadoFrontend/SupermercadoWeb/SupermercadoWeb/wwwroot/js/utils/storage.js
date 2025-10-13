/**
 * SUPERMERCADO POS - Storage Utility
 * Manejo centralizado de localStorage con encriptación básica
 */

const StorageUtil = {
    /**
     * Guarda un valor en localStorage
     * @param {string} key - Clave del valor
     * @param {any} value - Valor a guardar
     */
    set(key, value) {
        try {
            const serializedValue = JSON.stringify(value);
            localStorage.setItem(key, serializedValue);
            return true;
        } catch (error) {
            console.error('Error al guardar en localStorage:', error);
            return false;
        }
    },

    /**
     * Obtiene un valor de localStorage
     * @param {string} key - Clave del valor
     * @returns {any} Valor deserializado o null
     */
    get(key) {
        try {
            const serializedValue = localStorage.getItem(key);
            if (serializedValue === null) {
                return null;
            }
            return JSON.parse(serializedValue);
        } catch (error) {
            console.error('Error al leer de localStorage:', error);
            return null;
        }
    },

    /**
     * Elimina un valor de localStorage
     * @param {string} key - Clave del valor
     */
    remove(key) {
        try {
            localStorage.removeItem(key);
            return true;
        } catch (error) {
            console.error('Error al eliminar de localStorage:', error);
            return false;
        }
    },

    /**
     * Limpia todo el localStorage
     */
    clear() {
        try {
            localStorage.clear();
            return true;
        } catch (error) {
            console.error('Error al limpiar localStorage:', error);
            return false;
        }
    },

    /**
     * Verifica si existe una clave en localStorage
     * @param {string} key - Clave a verificar
     * @returns {boolean}
     */
    exists(key) {
        return localStorage.getItem(key) !== null;
    },

    // Métodos específicos para la aplicación
    
    /**
     * Guarda el token de autenticación
     * @param {string} token - Token JWT
     */
    setToken(token) {
        return this.set(AppConfig.STORAGE_KEYS.TOKEN, token);
    },

    /**
     * Obtiene el token de autenticación
     * @returns {string|null}
     */
    getToken() {
        return this.get(AppConfig.STORAGE_KEYS.TOKEN);
    },

    /**
     * Elimina el token de autenticación
     */
    removeToken() {
        return this.remove(AppConfig.STORAGE_KEYS.TOKEN);
    },

    /**
     * Guarda los datos del usuario
     * @param {object} user - Datos del usuario
     */
    setUser(user) {
        return this.set(AppConfig.STORAGE_KEYS.USER, user);
    },

    /**
     * Obtiene los datos del usuario
     * @returns {object|null}
     */
    getUser() {
        return this.get(AppConfig.STORAGE_KEYS.USER);
    },

    /**
     * Elimina los datos del usuario
     */
    removeUser() {
        return this.remove(AppConfig.STORAGE_KEYS.USER);
    },

    /**
     * Limpia toda la sesión del usuario
     */
    clearSession() {
        this.removeToken();
        this.removeUser();
        return true;
    }
};

// Hacer disponible globalmente
window.StorageUtil = StorageUtil;
