/**
 * SUPERMERCADO POS - Usuario Service
 * Servicio de gestión de usuarios
 */

class UsuarioService {
    constructor(apiClient) {
        this.apiClient = apiClient;
        this.endpoint = '/Usuario';
    }

    /**
     * Obtiene todos los usuarios
     * @returns {Promise<Array>}
     */
    async getAll() {
        try {
            return await this.apiClient.get(this.endpoint);
        } catch (error) {
            console.error('Error al obtener usuarios:', error);
            throw error;
        }
    }

    /**
     * Obtiene un usuario por ID
     * @param {number} id - ID del usuario
     * @returns {Promise<object>}
     */
    async getById(id) {
        try {
            return await this.apiClient.get(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al obtener usuario:', error);
            throw error;
        }
    }

    /**
     * Crea un nuevo usuario
     * @param {object} usuario - Datos del usuario
     * @returns {Promise<object>}
     */
    async create(usuario) {
        try {
            return await this.apiClient.post(this.endpoint, usuario);
        } catch (error) {
            console.error('Error al crear usuario:', error);
            throw error;
        }
    }

    /**
     * Actualiza un usuario existente
     * @param {number} id - ID del usuario
     * @param {object} usuario - Datos actualizados
     * @returns {Promise<object>}
     */
    async update(id, usuario) {
        try {
            const usuarioData = {
                usuarioId: id,
                ...usuario
            };
            return await this.apiClient.put(`${this.endpoint}/${id}`, usuarioData);
        } catch (error) {
            console.error('Error al actualizar usuario:', error);
            throw error;
        }
    }

    /**
     * Elimina un usuario
     * @param {number} id - ID del usuario
     * @returns {Promise<void>}
     */
    async delete(id) {
        try {
            return await this.apiClient.delete(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al eliminar usuario:', error);
            throw error;
        }
    }
}

// Crear instancia global del servicio
window.usuarioService = new UsuarioService(window.apiClient);
