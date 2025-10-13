/**
 * SUPERMERCADO POS - Auth Service
 * Servicio de autenticación (equivalente a AuthUnitOfWork del backend)
 */

class AuthService {
    constructor(apiClient) {
        this.apiClient = apiClient;
        this.endpoint = '/Auth';
    }

    /**
     * Inicia sesión
     * @param {string} email - Email del usuario
     * @param {string} password - Contraseña
     * @returns {Promise<object>}
     */
    async login(email, password) {
        try {
            const loginData = {
                email: email,
                password: password
            };

            const response = await this.apiClient.post(
                `${this.endpoint}/login`,
                loginData,
                false // No incluir auth en login
            );

            // Guardar token y usuario en localStorage
            if (response.accesToken) {
                StorageUtil.setToken(response.accesToken);
                StorageUtil.setUser({
                    email: response.email,
                    role: response.role,
                    expireIn: response.expireIn
                });
            }

            return response;
        } catch (error) {
            console.error('Error en login:', error);
            throw error;
        }
    }

    /**
     * Cierra sesión
     * @returns {Promise<void>}
     */
    async logout() {
        try {
            // Llamar al endpoint de logout si existe
            await this.apiClient.post(`${this.endpoint}/Logout`, {});
        } catch (error) {
            console.error('Error en logout:', error);
        } finally {
            // Siempre limpiar la sesión local
            StorageUtil.clearSession();
            // Limpiar la marca del splash para que se muestre en el próximo login
            sessionStorage.removeItem('welcomeSplashShown');
            window.location.href = '/login.html';
        }
    }

    /**
     * Obtiene el usuario actual
     * @returns {object|null}
     */
    getCurrentUser() {
        return StorageUtil.getUser();
    }

    /**
     * Verifica si el usuario está autenticado
     * @returns {boolean}
     */
    isAuthenticated() {
        return this.apiClient.isAuthenticated();
    }

    /**
     * Verifica si el usuario tiene un rol específico
     * @param {string} roleName - Nombre del rol
     * @returns {boolean}
     */
    hasRole(roleName) {
        const user = this.getCurrentUser();
        return user && user.rol === roleName;
    }

    /**
     * Protege una página (redirige a login si no está autenticado)
     */
    requireAuth() {
        if (!this.isAuthenticated()) {
            window.location.href = '/login.html';
        }
    }

    /**
     * Redirige al dashboard si ya está autenticado
     */
    redirectIfAuthenticated() {
        if (this.isAuthenticated()) {
            window.location.href = '/dashboard.html';
        }
    }
}

// Crear instancia global del servicio
window.authService = new AuthService(window.apiClient);
