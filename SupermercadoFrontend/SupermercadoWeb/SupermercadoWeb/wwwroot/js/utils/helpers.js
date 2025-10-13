/**
 * SUPERMERCADO POS - Helper Utilities
 * Funciones auxiliares y formatters
 */

const Helpers = {
    /**
     * Formatea un número como moneda
     * @param {number} amount - Cantidad a formatear
     * @param {string} currency - Código de moneda (default: 'COP')
     * @returns {string}
     */
    formatCurrency(amount, currency = 'COP') {
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: currency,
            minimumFractionDigits: 2
        }).format(amount);
    },

    /**
     * Formatea una fecha
     * @param {string|Date} date - Fecha a formatear
     * @param {boolean} includeTime - Incluir hora
     * @returns {string}
     */
    formatDate(date, includeTime = false) {
        if (!date) return '';
        const dateObj = new Date(date);
        const options = {
            year: 'numeric',
            month: '2-digit',
            day: '2-digit'
        };
        if (includeTime) {
            options.hour = '2-digit';
            options.minute = '2-digit';
        }
        return dateObj.toLocaleDateString('es-CO', options);
    },

    /**
     * Formatea un número con separadores de miles
     * @param {number} number - Número a formatear
     * @returns {string}
     */
    formatNumber(number) {
        return new Intl.NumberFormat('es-CO').format(number);
    },

    /**
     * Capitaliza la primera letra de un string
     * @param {string} str - String a capitalizar
     * @returns {string}
     */
    capitalize(str) {
        if (!str) return '';
        return str.charAt(0).toUpperCase() + str.slice(1).toLowerCase();
    },

    /**
     * Muestra un toast de notificación
     * @param {string} message - Mensaje a mostrar
     * @param {string} type - Tipo: success, error, warning, info
     */
    showToast(message, type = 'info') {
        // Crear contenedor de toasts si no existe
        let toastContainer = document.getElementById('toast-container');
        if (!toastContainer) {
            toastContainer = document.createElement('div');
            toastContainer.id = 'toast-container';
            toastContainer.className = 'position-fixed top-0 end-0 p-3';
            toastContainer.style.zIndex = '9999';
            document.body.appendChild(toastContainer);
        }

        // Mapear tipos a clases de Bootstrap
        const typeClasses = {
            success: 'bg-success text-white',
            error: 'bg-danger text-white',
            warning: 'bg-warning text-dark',
            info: 'bg-info text-white'
        };

        // Crear toast
        const toastId = 'toast-' + Date.now();
        const toastHTML = `
            <div id="${toastId}" class="toast align-items-center ${typeClasses[type] || typeClasses.info} border-0" role="alert" aria-live="assertive" aria-atomic="true">
                <div class="d-flex">
                    <div class="toast-body">
                        ${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
                </div>
            </div>
        `;

        toastContainer.insertAdjacentHTML('beforeend', toastHTML);
        const toastElement = document.getElementById(toastId);
        const toast = new bootstrap.Toast(toastElement, { delay: 3000 });
        toast.show();

        // Eliminar del DOM después de ocultarse
        toastElement.addEventListener('hidden.bs.toast', () => {
            toastElement.remove();
        });
    },

    /**
     * Muestra un modal de confirmación
     * @param {string} title - Título del modal
     * @param {string} message - Mensaje del modal
     * @param {function} onConfirm - Callback al confirmar
     * @param {function} onCancel - Callback al cancelar
     */
    showConfirmModal(title, message, onConfirm, onCancel = null) {
        // Crear modal si no existe
        let modal = document.getElementById('confirm-modal');
        if (!modal) {
            const modalHTML = `
                <div class="modal fade" id="confirm-modal" tabindex="-1">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title" id="confirm-modal-title"></h5>
                                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                            </div>
                            <div class="modal-body" id="confirm-modal-body"></div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                                <button type="button" class="btn btn-primary" id="confirm-modal-btn">Confirmar</button>
                            </div>
                        </div>
                    </div>
                </div>
            `;
            document.body.insertAdjacentHTML('beforeend', modalHTML);
            modal = document.getElementById('confirm-modal');
        }

        // Actualizar contenido
        document.getElementById('confirm-modal-title').textContent = title;
        document.getElementById('confirm-modal-body').textContent = message;

        // Configurar eventos
        const confirmBtn = document.getElementById('confirm-modal-btn');
        const newConfirmBtn = confirmBtn.cloneNode(true);
        confirmBtn.parentNode.replaceChild(newConfirmBtn, confirmBtn);

        newConfirmBtn.addEventListener('click', () => {
            const bsModal = bootstrap.Modal.getInstance(modal);
            bsModal.hide();
            if (onConfirm) onConfirm();
        });

        modal.addEventListener('hidden.bs.modal', () => {
            if (onCancel) onCancel();
        }, { once: true });

        // Mostrar modal
        const bsModal = new bootstrap.Modal(modal);
        bsModal.show();
    },

    /**
     * Muestra un loading spinner
     * @param {boolean} show - true para mostrar, false para ocultar
     */
    showLoading(show = true) {
        let loader = document.getElementById('global-loader');
        if (!loader) {
            const loaderHTML = `
                <div id="global-loader" class="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" style="background: rgba(0,0,0,0.5); z-index: 9999; display: none;">
                    <div class="spinner-border text-light" role="status" style="width: 3rem; height: 3rem;">
                        <span class="visually-hidden">Cargando...</span>
                    </div>
                </div>
            `;
            document.body.insertAdjacentHTML('beforeend', loaderHTML);
            loader = document.getElementById('global-loader');
        }
        loader.style.display = show ? 'flex' : 'none';
    },

    /**
     * Debounce function para optimizar búsquedas
     * @param {function} func - Función a ejecutar
     * @param {number} wait - Tiempo de espera en ms
     * @returns {function}
     */
    debounce(func, wait = 300) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    },

    /**
     * Genera un ID único
     * @returns {string}
     */
    generateId() {
        return Date.now().toString(36) + Math.random().toString(36).substr(2);
    },

    /**
     * Copia texto al portapapeles
     * @param {string} text - Texto a copiar
     */
    async copyToClipboard(text) {
        try {
            await navigator.clipboard.writeText(text);
            this.showToast('Copiado al portapapeles', 'success');
        } catch (err) {
            console.error('Error al copiar:', err);
            this.showToast('Error al copiar', 'error');
        }
    },

    /**
     * Descarga datos como archivo
     * @param {string} data - Datos a descargar
     * @param {string} filename - Nombre del archivo
     * @param {string} type - Tipo MIME
     */
    downloadFile(data, filename, type = 'text/plain') {
        const blob = new Blob([data], { type });
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
    }
};

// Hacer disponible globalmente
window.Helpers = Helpers;
