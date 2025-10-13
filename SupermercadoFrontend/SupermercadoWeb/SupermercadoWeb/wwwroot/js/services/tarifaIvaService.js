/**
 * SUPERMERCADO POS - Tarifa IVA Service
 * Servicio de tarifas de IVA
 */

class TarifaIvaService {
    constructor(apiClient) {
        this.apiClient = apiClient;
        this.endpoint = '/TarifaIva';
    }

    /**
     * Obtiene todas las tarifas de IVA
     * @returns {Promise<Array>}
     */
    async getAll() {
        try {
            return await this.apiClient.get(this.endpoint);
        } catch (error) {
            console.error('Error al obtener tarifas de IVA:', error);
            throw error;
        }
    }

    /**
     * Obtiene solo las tarifas de IVA activas
     * @returns {Promise<Array>}
     */
    async getAllActivos() {
        try {
            return await this.apiClient.get(`${this.endpoint}/activas`);
        } catch (error) {
            console.error('Error al obtener tarifas de IVA activas:', error);
            throw error;
        }
    }

    /**
     * Obtiene una tarifa de IVA por ID
     * @param {number} id - ID de la tarifa
     * @returns {Promise<object>}
     */
    async getById(id) {
        try {
            return await this.apiClient.get(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al obtener tarifa de IVA:', error);
            throw error;
        }
    }

    /**
     * Crea una nueva tarifa de IVA
     * @param {object} tarifa - Datos de la tarifa
     * @returns {Promise<object>}
     */
    async create(tarifa) {
        try {
            return await this.apiClient.post(this.endpoint, tarifa);
        } catch (error) {
            console.error('Error al crear tarifa de IVA:', error);
            throw error;
        }
    }

    /**
     * Actualiza una tarifa de IVA existente
     * @param {number} id - ID de la tarifa
     * @param {object} tarifa - Datos actualizados
     * @returns {Promise<object>}
     */
    async update(id, tarifa) {
        try {
            // Agregar el ID al objeto tarifa
            const tarifaData = {
                TarifaIvaId: id,
                ...tarifa
            };
            return await this.apiClient.put(`${this.endpoint}/${id}`, tarifaData);
        } catch (error) {
            console.error('Error al actualizar tarifa de IVA:', error);
            throw error;
        }
    }

    /**
     * Elimina una tarifa de IVA
     * @param {number} id - ID de la tarifa
     * @returns {Promise<void>}
     */
    async delete(id) {
        try {
            return await this.apiClient.delete(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al eliminar tarifa de IVA:', error);
            throw error;
        }
    }
}

// Crear instancia global del servicio
window.tarifaIvaService = new TarifaIvaService(window.apiClient);
