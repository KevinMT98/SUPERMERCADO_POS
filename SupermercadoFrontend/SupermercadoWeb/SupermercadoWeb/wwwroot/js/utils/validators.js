/**
 * SUPERMERCADO POS - Validators Utility
 * Funciones de validación reutilizables
 */

const Validators = {
    /**
     * Valida si un campo está vacío
     * @param {string} value - Valor a validar
     * @returns {boolean}
     */
    isEmpty(value) {
        return !value || value.trim() === '';
    },

    /**
     * Valida formato de email
     * @param {string} email - Email a validar
     * @returns {boolean}
     */
    isValidEmail(email) {
        if (this.isEmpty(email)) return false;
        return AppConfig.VALIDATION.EMAIL_REGEX.test(email);
    },

    /**
     * Valida longitud mínima
     * @param {string} value - Valor a validar
     * @param {number} minLength - Longitud mínima
     * @returns {boolean}
     */
    hasMinLength(value, minLength) {
        return value && value.length >= minLength;
    },

    /**
     * Valida longitud máxima
     * @param {string} value - Valor a validar
     * @param {number} maxLength - Longitud máxima
     * @returns {boolean}
     */
    hasMaxLength(value, maxLength) {
        return value && value.length <= maxLength;
    },

    /**
     * Valida contraseña
     * @param {string} password - Contraseña a validar
     * @returns {object} { isValid: boolean, message: string }
     */
    validatePassword(password) {
        if (this.isEmpty(password)) {
            return { isValid: false, message: 'La contraseña es requerida' };
        }
        if (!this.hasMinLength(password, AppConfig.VALIDATION.MIN_PASSWORD_LENGTH)) {
            return { 
                isValid: false, 
                message: `La contraseña debe tener al menos ${AppConfig.VALIDATION.MIN_PASSWORD_LENGTH} caracteres` 
            };
        }
        return { isValid: true, message: '' };
    },

    /**
     * Valida número positivo
     * @param {number} value - Valor a validar
     * @returns {boolean}
     */
    isPositiveNumber(value) {
        return !isNaN(value) && value > 0;
    },

    /**
     * Valida número entero
     * @param {number} value - Valor a validar
     * @returns {boolean}
     */
    isInteger(value) {
        return Number.isInteger(Number(value));
    },

    /**
     * Valida rango de números
     * @param {number} value - Valor a validar
     * @param {number} min - Valor mínimo
     * @param {number} max - Valor máximo
     * @returns {boolean}
     */
    isInRange(value, min, max) {
        const num = Number(value);
        return !isNaN(num) && num >= min && num <= max;
    },

    /**
     * Valida formulario completo
     * @param {HTMLFormElement} form - Formulario a validar
     * @returns {object} { isValid: boolean, errors: array }
     */
    validateForm(form) {
        const errors = [];
        const formData = new FormData(form);

        // Validar campos requeridos
        form.querySelectorAll('[required]').forEach(field => {
            const value = formData.get(field.name);
            if (this.isEmpty(value)) {
                errors.push({
                    field: field.name,
                    message: `El campo ${field.placeholder || field.name} es requerido`
                });
            }
        });

        // Validar emails
        form.querySelectorAll('input[type="email"]').forEach(field => {
            const value = formData.get(field.name);
            if (!this.isEmpty(value) && !this.isValidEmail(value)) {
                errors.push({
                    field: field.name,
                    message: 'El formato del email no es válido'
                });
            }
        });

        // Validar números
        form.querySelectorAll('input[type="number"]').forEach(field => {
            const value = formData.get(field.name);
            const min = field.getAttribute('min');
            const max = field.getAttribute('max');
            
            if (!this.isEmpty(value)) {
                if (min && Number(value) < Number(min)) {
                    errors.push({
                        field: field.name,
                        message: `El valor mínimo es ${min}`
                    });
                }
                if (max && Number(value) > Number(max)) {
                    errors.push({
                        field: field.name,
                        message: `El valor máximo es ${max}`
                    });
                }
            }
        });

        return {
            isValid: errors.length === 0,
            errors: errors
        };
    },

    /**
     * Muestra errores de validación en el formulario
     * @param {HTMLFormElement} form - Formulario
     * @param {array} errors - Array de errores
     */
    showFormErrors(form, errors) {
        // Limpiar errores previos
        form.querySelectorAll('.is-invalid').forEach(el => {
            el.classList.remove('is-invalid');
        });
        form.querySelectorAll('.invalid-feedback').forEach(el => {
            el.remove();
        });

        // Mostrar nuevos errores
        errors.forEach(error => {
            const field = form.querySelector(`[name="${error.field}"]`);
            if (field) {
                field.classList.add('is-invalid');
                const feedback = document.createElement('div');
                feedback.className = 'invalid-feedback';
                feedback.textContent = error.message;
                field.parentNode.appendChild(feedback);
            }
        });
    }
};

// Hacer disponible globalmente
window.Validators = Validators;
