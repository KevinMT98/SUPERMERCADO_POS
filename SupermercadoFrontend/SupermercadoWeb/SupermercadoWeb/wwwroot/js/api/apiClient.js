/**
 * SUPERMERCADO POS - API Client
 * Capa de comunicación HTTP con el backend
 * Similar al patrón Repository del backend
 */

class ApiClient {
    constructor(baseURL) {
        this.baseURL = baseURL || AppConfig.API_BASE_URL;
    }

    /**
     * Obtiene los headers para las peticiones
     * @param {boolean} includeAuth - Incluir token de autenticación
     * @returns {object}
     */
    getHeaders(includeAuth = true) {
        const headers = {
            'Content-Type': 'application/json'
        };

        if (includeAuth) {
            const token = StorageUtil.getToken();
            if (token) {
                headers['Authorization'] = `Bearer ${token}`;
            }
        }

        return headers;
    }

    /**
     * Maneja errores de las peticiones HTTP
     * @param {Response} response - Respuesta HTTP
     * @returns {Promise}
     */
    async handleResponse(response) {
        if (response.ok) {
            // Si es 204 No Content, retornar objeto vacío
            if (response.status === 204) {
                return { wasSuccess: true };
            }
            return await response.json();
        }

        // Manejar errores
        let errorMessage = AppConfig.MESSAGES.ERROR.GENERIC;

        if (response.status === 401) {
            errorMessage = AppConfig.MESSAGES.ERROR.SESSION_EXPIRED;
            // Limpiar sesión y redirigir al login
            StorageUtil.clearSession();
            window.location.href = '/login.html';
        } else if (response.status === 403) {
            errorMessage = AppConfig.MESSAGES.ERROR.UNAUTHORIZED;
        } else if (response.status === 404) {
            errorMessage = 'Recurso no encontrado';
        } else if (response.status >= 500) {
            errorMessage = 'Error del servidor. Por favor, intente más tarde';
        } else {
            // Intentar obtener mensaje del servidor
            try {
                // Primero intentar leer como texto
                const contentType = response.headers.get('content-type');
                let errorData;
                
                if (contentType && contentType.includes('application/json')) {
                    errorData = await response.json();
                } else {
                    // Si no es JSON, leer como texto
                    const textError = await response.text();
                    if (textError) {
                        errorMessage = textError;
                        throw new Error(errorMessage);
                    }
                }
                
                console.error('Error del servidor:', errorData);
                
                // Intentar obtener el mensaje de error del JSON
                if (errorData.errors) {
                    // Errores de validación de ModelState
                    const validationErrors = Object.values(errorData.errors).flat();
                    errorMessage = validationErrors.join(', ');
                } else if (errorData.message || errorData.Message) {
                    errorMessage = errorData.message || errorData.Message;
                } else if (typeof errorData === 'string') {
                    errorMessage = errorData;
                }
            } catch (e) {
                // Si ya es un Error con mensaje, lanzarlo
                if (e instanceof Error && e.message !== errorMessage) {
                    throw e;
                }
                console.error('Error al procesar respuesta:', e);
            }
        }

        throw new Error(errorMessage);
    }

    /**
     * Realiza una petición GET
     * @param {string} endpoint - Endpoint de la API
     * @param {object} params - Parámetros de query string
     * @param {boolean} includeAuth - Incluir autenticación
     * @returns {Promise}
     */
    async get(endpoint, params = null, includeAuth = true) {
        try {
            let url = `${this.baseURL}${endpoint}`;
            
            // Agregar parámetros de query string
            if (params) {
                const queryString = new URLSearchParams(params).toString();
                url += `?${queryString}`;
            }

            const response = await fetch(url, {
                method: 'GET',
                headers: this.getHeaders(includeAuth)
            });

            return await this.handleResponse(response);
        } catch (error) {
            console.error('Error en GET:', error);
            throw error;
        }
    }

    /**
     * Realiza una petición POST
     * @param {string} endpoint - Endpoint de la API
     * @param {object} data - Datos a enviar
     * @param {boolean} includeAuth - Incluir autenticación
     * @returns {Promise}
     */
    async post(endpoint, data, includeAuth = true) {
        try {
            const response = await fetch(`${this.baseURL}${endpoint}`, {
                method: 'POST',
                headers: this.getHeaders(includeAuth),
                body: JSON.stringify(data)
            });

            return await this.handleResponse(response);
        } catch (error) {
            console.error('Error en POST:', error);
            throw error;
        }
    }

    /**
     * Realiza una petición PUT
     * @param {string} endpoint - Endpoint de la API
     * @param {object} data - Datos a enviar
     * @param {boolean} includeAuth - Incluir autenticación
     * @returns {Promise}
     */
    async put(endpoint, data, includeAuth = true) {
        try {
            const response = await fetch(`${this.baseURL}${endpoint}`, {
                method: 'PUT',
                headers: this.getHeaders(includeAuth),
                body: JSON.stringify(data)
            });

            return await this.handleResponse(response);
        } catch (error) {
            console.error('Error en PUT:', error);
            throw error;
        }
    }

    /**
     * Realiza una petición DELETE
     * @param {string} endpoint - Endpoint de la API
     * @param {boolean} includeAuth - Incluir autenticación
     * @returns {Promise}
     */
    async delete(endpoint, includeAuth = true) {
        try {
            const response = await fetch(`${this.baseURL}${endpoint}`, {
                method: 'DELETE',
                headers: this.getHeaders(includeAuth)
            });

            return await this.handleResponse(response);
        } catch (error) {
            console.error('Error en DELETE:', error);
            throw error;
        }
    }

    /**
     * Verifica si el usuario está autenticado
     * @returns {boolean}
     */
    isAuthenticated() {
        return StorageUtil.getToken() !== null;
    }
}

// Crear instancia global del API Client
window.apiClient = new ApiClient();
