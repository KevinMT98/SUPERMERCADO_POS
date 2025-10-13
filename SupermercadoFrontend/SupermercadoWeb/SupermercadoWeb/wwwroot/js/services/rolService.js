/**
 * SUPERMERCADO POS - Rol Service
 * Servicio de gestión de roles
 */

class RolService {
    constructor(apiClient) {
        this.apiClient = apiClient;
        this.endpoint = '/Rols';
    }

    /**
     * Obtiene todos los roles
     * @returns {Promise<Array>}
     */
    async getAll() {
        try {
            return await this.apiClient.get(this.endpoint);
        } catch (error) {
            console.error('Error al obtener roles:', error);
            throw error;
        }
    }

    /**
     * Obtiene solo los roles activos
     * @returns {Promise<Array>}
     */
    async getAllActivos() {
        try {
            return await this.apiClient.get(`${this.endpoint}/activos`);
        } catch (error) {
            console.error('Error al obtener roles activos:', error);
            throw error;
        }
    }

    /**
     * Obtiene un rol por ID
     * @param {number} id - ID del rol
     * @returns {Promise<object>}
     */
    async getById(id) {
        try {
            return await this.apiClient.get(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al obtener rol:', error);
            throw error;
        }
    }

    /**
     * Crea un nuevo rol
     * @param {object} rol - Datos del rol
     * @returns {Promise<object>}
     */
    async create(rol) {
        try {
            return await this.apiClient.post(this.endpoint, rol);
        } catch (error) {
            console.error('Error al crear rol:', error);
            throw error;
        }
    }

    /**
     * Actualiza un rol existente
     * @param {number} id - ID del rol
     * @param {object} rol - Datos actualizados
     * @returns {Promise<object>}
     */
    async update(id, rol) {
        try {
            // Agregar el ID al objeto rol
            const rolData = {
                rol_id: id,
                ...rol
            };
            return await this.apiClient.put(`${this.endpoint}/${id}`, rolData);
        } catch (error) {
            console.error('Error al actualizar rol:', error);
            throw error;
        }
    }

    /**
     * Elimina un rol
     * @param {number} id - ID del rol
     * @returns {Promise<void>}
     */
    async delete(id) {
        try {
            return await this.apiClient.delete(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al eliminar rol:', error);
            throw error;
        }
    }
}

// Crear instancia global del servicio
window.rolService = new RolService(window.apiClient);
