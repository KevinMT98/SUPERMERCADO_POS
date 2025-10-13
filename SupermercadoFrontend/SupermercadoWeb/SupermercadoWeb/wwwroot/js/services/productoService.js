/**
 * SUPERMERCADO POS - Producto Service
 * Servicio de productos (equivalente a ProductoUnitOfWork del backend)
 */

class ProductoService {
    constructor(apiClient) {
        this.apiClient = apiClient;
        this.endpoint = '/Producto';
    }

    /**
     * Obtiene todos los productos
     * @returns {Promise<Array>}
     */
    async getAll() {
        try {
            return await this.apiClient.get(this.endpoint);
        } catch (error) {
            console.error('Error al obtener productos:', error);
            throw error;
        }
    }

    /**
     * Obtiene un producto por ID
     * @param {number} id - ID del producto
     * @returns {Promise<object>}
     */
    async getById(id) {
        try {
            return await this.apiClient.get(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al obtener producto:', error);
            throw error;
        }
    }

    /**
     * Crea un nuevo producto
     * @param {object} producto - Datos del producto
     * @returns {Promise<object>}
     */
    async create(producto) {
        try {
            // Validar campos numéricos requeridos
            const categoriaId = parseInt(producto.categoriaId);
            const codigoIva = parseInt(producto.codigoIva);
            
            if (isNaN(categoriaId)) {
                throw new Error('Debe seleccionar una categoría');
            }
            
            if (isNaN(codigoIva)) {
                throw new Error('Debe seleccionar una tarifa de IVA');
            }
            
            const productoData = {
                codigoProducto: producto.codigoProducto,
                codigoBarras: producto.codigoBarras,
                nombre: producto.nombre,
                descripcion: producto.descripcion || '',
                precioUnitario: parseFloat(producto.precioUnitario) || 0,
                categoriaId: categoriaId,
                codigoIva: codigoIva,
                stockActual: parseInt(producto.stockActual) || 0,
                stockMinimo: parseInt(producto.stockMinimo) || 0,
                stockMaximo: parseInt(producto.stockMaximo) || 0,
                activo: producto.activo !== undefined ? producto.activo : true
            };

            return await this.apiClient.post(this.endpoint, productoData);
        } catch (error) {
            console.error('Error al crear producto:', error);
            throw error;
        }
    }

    /**
     * Actualiza un producto existente
     * @param {number} id - ID del producto
     * @param {object} producto - Datos actualizados
     * @returns {Promise<object>}
     */
    async update(id, producto) {
        try {
            // Validar campos numéricos requeridos
            const categoriaId = parseInt(producto.categoriaId);
            const codigoIva = parseInt(producto.codigoIva);
            
            if (isNaN(categoriaId)) {
                throw new Error('Debe seleccionar una categoría');
            }
            
            if (isNaN(codigoIva)) {
                throw new Error('Debe seleccionar una tarifa de IVA');
            }
            
            const productoData = {
                productoId: id,
                codigoProducto: producto.codigoProducto,
                codigoBarras: producto.codigoBarras,
                nombre: producto.nombre,
                descripcion: producto.descripcion || '',
                precioUnitario: parseFloat(producto.precioUnitario) || 0,
                categoriaId: categoriaId,
                codigoIva: codigoIva,
                stockActual: parseInt(producto.stockActual) || 0,
                stockMinimo: parseInt(producto.stockMinimo) || 0,
                stockMaximo: parseInt(producto.stockMaximo) || 0,
                activo: producto.activo
            };

            return await this.apiClient.put(`${this.endpoint}/${id}`, productoData);
        } catch (error) {
            console.error('Error al actualizar producto:', error);
            throw error;
        }
    }

    /**
     * Elimina un producto
     * @param {number} id - ID del producto
     * @returns {Promise<void>}
     */
    async delete(id) {
        try {
            return await this.apiClient.delete(`${this.endpoint}/${id}`);
        } catch (error) {
            console.error('Error al eliminar producto:', error);
            throw error;
        }
    }

    /**
     * Busca productos por nombre o código
     * @param {string} searchTerm - Término de búsqueda
     * @returns {Promise<Array>}
     */
    async search(searchTerm) {
        try {
            const productos = await this.getAll();
            if (!searchTerm) return productos;

            const term = searchTerm.toLowerCase();
            return productos.filter(p => 
                p.nombre.toLowerCase().includes(term) ||
                p.codigoProducto.toLowerCase().includes(term) ||
                p.codigoBarras.toLowerCase().includes(term)
            );
        } catch (error) {
            console.error('Error al buscar productos:', error);
            throw error;
        }
    }

    /**
     * Obtiene productos con stock bajo
     * @returns {Promise<Array>}
     */
    async getProductosConStockBajo() {
        try {
            const productos = await this.getAll();
            return productos.filter(p => p.stockActual < p.stockMinimo);
        } catch (error) {
            console.error('Error al obtener productos con stock bajo:', error);
            throw error;
        }
    }

    /**
     * Obtiene productos por categoría
     * @param {number} categoriaId - ID de la categoría
     * @returns {Promise<Array>}
     */
    async getByCategoria(categoriaId) {
        try {
            const productos = await this.getAll();
            return productos.filter(p => p.categoriaId === categoriaId);
        } catch (error) {
            console.error('Error al obtener productos por categoría:', error);
            throw error;
        }
    }
}

// Crear instancia global del servicio
window.productoService = new ProductoService(window.apiClient);
