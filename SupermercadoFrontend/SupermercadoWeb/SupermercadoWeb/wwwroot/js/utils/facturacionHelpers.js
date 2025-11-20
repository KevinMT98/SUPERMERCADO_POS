/**
 * SUPERMERCADO POS - Facturación Helpers
 * Utilidades específicas para el módulo de facturación
 */

class FacturacionHelpers {
    /**
     * Formatea un número como moneda colombiana
     * @param {number} valor - Valor a formatear
     * @returns {string} Valor formateado
     */
    static formatearMoneda(valor) {
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
            maximumFractionDigits: 0
        }).format(valor || 0);
    }

    /**
     * Formatea un número con separadores de miles
     * @param {number} valor - Valor a formatear
     * @returns {string} Valor formateado
     */
    static formatearNumero(valor) {
        return new Intl.NumberFormat('es-CO').format(valor || 0);
    }

    /**
     * Valida si un número es válido para cálculos monetarios
     * @param {any} valor - Valor a validar
     * @returns {boolean} True si es válido
     */
    static esNumeroValido(valor) {
        const num = parseFloat(valor);
        return !isNaN(num) && isFinite(num) && num >= 0;
    }

    /**
     * Redondea un valor monetario a 2 decimales
     * @param {number} valor - Valor a redondear
     * @returns {number} Valor redondeado
     */
    static redondearMoneda(valor) {
        return Math.round((valor || 0) * 100) / 100;
    }

    /**
     * Calcula el porcentaje de un valor
     * @param {number} valor - Valor base
     * @param {number} porcentaje - Porcentaje a calcular
     * @returns {number} Resultado del cálculo
     */
    static calcularPorcentaje(valor, porcentaje) {
        return this.redondearMoneda((valor || 0) * ((porcentaje || 0) / 100));
    }

    /**
     * Genera un color aleatorio para badges
     * @returns {string} Clase CSS de color
     */
    static generarColorBadge() {
        const colores = ['primary', 'secondary', 'success', 'info', 'warning'];
        return colores[Math.floor(Math.random() * colores.length)];
    }

    /**
     * Formatea una fecha para mostrar
     * @param {Date|string} fecha - Fecha a formatear
     * @returns {string} Fecha formateada
     */
    static formatearFecha(fecha) {
        if (!fecha) return '';
        
        const date = new Date(fecha);
        return new Intl.DateTimeFormat('es-CO', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit',
            hour: '2-digit',
            minute: '2-digit'
        }).format(date);
    }

    /**
     * Formatea solo la fecha sin hora
     * @param {Date|string} fecha - Fecha a formatear
     * @returns {string} Fecha formateada
     */
    static formatearSoloFecha(fecha) {
        if (!fecha) return '';
        
        const date = new Date(fecha);
        return new Intl.DateTimeFormat('es-CO', {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
        }).format(date);
    }

    /**
     * Valida si una cadena es un email válido
     * @param {string} email - Email a validar
     * @returns {boolean} True si es válido
     */
    static esEmailValido(email) {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return regex.test(email);
    }

    /**
     * Genera un ID único para elementos temporales
     * @returns {string} ID único
     */
    static generarIdUnico() {
        return 'temp_' + Date.now() + '_' + Math.random().toString(36).substr(2, 9);
    }

    /**
     * Debounce para búsquedas
     * @param {Function} func - Función a ejecutar
     * @param {number} delay - Delay en milisegundos
     * @returns {Function} Función con debounce
     */
    static debounce(func, delay) {
        let timeoutId;
        return function (...args) {
            clearTimeout(timeoutId);
            timeoutId = setTimeout(() => func.apply(this, args), delay);
        };
    }

    /**
     * Capitaliza la primera letra de una cadena
     * @param {string} str - Cadena a capitalizar
     * @returns {string} Cadena capitalizada
     */
    static capitalizar(str) {
        if (!str) return '';
        return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
    }

    /**
     * Trunca un texto a una longitud específica
     * @param {string} texto - Texto a truncar
     * @param {number} longitud - Longitud máxima
     * @returns {string} Texto truncado
     */
    static truncarTexto(texto, longitud = 50) {
        if (!texto) return '';
        return texto.length > longitud ? texto.substring(0, longitud) + '...' : texto;
    }

    /**
     * Convierte un objeto a query string
     * @param {object} obj - Objeto a convertir
     * @returns {string} Query string
     */
    static objetoAQueryString(obj) {
        const params = new URLSearchParams();
        for (const [key, value] of Object.entries(obj)) {
            if (value !== null && value !== undefined && value !== '') {
                params.append(key, value);
            }
        }
        return params.toString();
    }

    /**
     * Valida si un objeto tiene todas las propiedades requeridas
     * @param {object} obj - Objeto a validar
     * @param {Array} propiedadesRequeridas - Array de propiedades requeridas
     * @returns {object} Resultado de validación
     */
    static validarPropiedadesRequeridas(obj, propiedadesRequeridas) {
        const faltantes = propiedadesRequeridas.filter(prop => 
            !obj.hasOwnProperty(prop) || obj[prop] === null || obj[prop] === undefined || obj[prop] === ''
        );

        return {
            esValido: faltantes.length === 0,
            propiedadesFaltantes: faltantes
        };
    }

    /**
     * Crea un elemento HTML con clases y atributos
     * @param {string} tag - Tag del elemento
     * @param {object} options - Opciones del elemento
     * @returns {HTMLElement} Elemento creado
     */
    static crearElemento(tag, options = {}) {
        const elemento = document.createElement(tag);
        
        if (options.clases) {
            elemento.className = Array.isArray(options.clases) ? options.clases.join(' ') : options.clases;
        }
        
        if (options.atributos) {
            Object.entries(options.atributos).forEach(([attr, value]) => {
                elemento.setAttribute(attr, value);
            });
        }
        
        if (options.contenido) {
            elemento.innerHTML = options.contenido;
        }
        
        if (options.texto) {
            elemento.textContent = options.texto;
        }
        
        return elemento;
    }

    /**
     * Anima un elemento con una clase CSS
     * @param {HTMLElement} elemento - Elemento a animar
     * @param {string} animacion - Clase de animación
     * @param {Function} callback - Callback al finalizar
     */
    static animarElemento(elemento, animacion, callback) {
        elemento.classList.add(animacion);
        
        const handleAnimationEnd = () => {
            elemento.classList.remove(animacion);
            elemento.removeEventListener('animationend', handleAnimationEnd);
            if (callback) callback();
        };
        
        elemento.addEventListener('animationend', handleAnimationEnd);
    }

    /**
     * Copia texto al portapapeles
     * @param {string} texto - Texto a copiar
     * @returns {Promise<boolean>} True si se copió exitosamente
     */
    static async copiarAlPortapapeles(texto) {
        try {
            await navigator.clipboard.writeText(texto);
            return true;
        } catch (error) {
            console.error('Error al copiar al portapapeles:', error);
            return false;
        }
    }

    /**
     * Descarga un archivo JSON
     * @param {object} data - Datos a descargar
     * @param {string} filename - Nombre del archivo
     */
    static descargarJSON(data, filename = 'data.json') {
        const blob = new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
        
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }

    /**
     * Obtiene el estado de una factura basado en sus datos
     * @param {object} factura - Datos de la factura
     * @returns {object} Estado con clase y texto
     */
    static obtenerEstadoFactura(factura) {
        if (factura.anulada) {
            return { clase: 'danger', texto: 'Anulada', icono: 'x-circle' };
        }
        
        if (factura.pagada) {
            return { clase: 'success', texto: 'Pagada', icono: 'check-circle' };
        }
        
        return { clase: 'warning', texto: 'Pendiente', icono: 'clock' };
    }

    /**
     * Calcula la diferencia en días entre dos fechas
     * @param {Date|string} fecha1 - Primera fecha
     * @param {Date|string} fecha2 - Segunda fecha
     * @returns {number} Diferencia en días
     */
    static calcularDiferenciaDias(fecha1, fecha2) {
        const d1 = new Date(fecha1);
        const d2 = new Date(fecha2);
        const diffTime = Math.abs(d2 - d1);
        return Math.ceil(diffTime / (1000 * 60 * 60 * 24));
    }

    /**
     * Valida si una fecha está dentro de un rango permitido
     * @param {Date|string} fecha - Fecha a validar
     * @param {number} diasMaximos - Días máximos permitidos desde hoy
     * @returns {boolean} True si está dentro del rango
     */
    static esFechaValida(fecha, diasMaximos = 30) {
        const hoy = new Date();
        const fechaValidar = new Date(fecha);
        const diferencia = this.calcularDiferenciaDias(hoy, fechaValidar);
        return diferencia <= diasMaximos;
    }
}

// Hacer disponible globalmente
window.FacturacionHelpers = FacturacionHelpers;
