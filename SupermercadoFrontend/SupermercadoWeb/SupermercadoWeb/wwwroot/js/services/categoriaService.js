/**
 * SUPERMERCADO POS - Categoria Service
 * Servicio de categorías de productos
 */

class CategoriaService {
    constructor(apiClient) {
        this.apiClient = apiClient;
        this.endpoint = '/Categoria_Producto';
    }

    /**
     * Obtiene todas las categorías
     * @returns {Promise<Array>}
     */
    async getAll() {
        try {
            return await this.apiClient.get(this.endpoint);
        } catch (error) {
            console.error('Error al obtener categorías:', error);
            throw error;
        }
    }

    /**
     * Obtiene solo las categorías activas
     * @returns {Promise<Array>}
     */
    async getAllActivos() {
        try {
            return await this.apiClient.get(`${this.endpoint}/activas`);
        } catch (error) {
            console.error('Error al obtener categorías activas:', error);
            throw error;
        }
    }

    /**
     * Obtiene una categoría por ID
     * @param {number} id - ID de la categoría
     * @returns {Promise<object>}
     */
    async getById(id) {
        try {
            return await this.apiClient.get(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al obtener categoría:', error);
            throw error;
        }
    }

    /**
     * Crea una nueva categoría
     * @param {object} categoria - Datos de la categoría
     * @returns {Promise<object>}
     */
    async create(categoria) {
        try {
            return await this.apiClient.post(this.endpoint, categoria);
        } catch (error) {
            console.error('Error al crear categoría:', error);
            throw error;
        }
    }

    /**
     * Actualiza una categoría existente
     * @param {number} id - ID de la categoría
     * @param {object} categoria - Datos actualizados
     * @returns {Promise<object>}
     */
    async update(id, categoria) {
        try {
            // Agregar el ID al objeto categoría
            const categoriaData = {
                CategoriaId: id,
                ...categoria
            };
            return await this.apiClient.put(`${this.endpoint}/${id}`, categoriaData);
        } catch (error) {
            console.error('Error al actualizar categoría:', error);
            throw error;
        }
    }

    /**
     * Elimina una categoría
     * @param {number} id - ID de la categoría
     * @returns {Promise<void>}
     */
    async delete(id) {
        try {
            return await this.apiClient.delete(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al eliminar categoría:', error);
            throw error;
        }
    }
}

// Crear instancia global del servicio
window.categoriaService = new CategoriaService(window.apiClient);
