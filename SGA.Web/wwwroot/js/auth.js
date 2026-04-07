class AuthService {
    constructor() {
        this.TOKEN_KEY = 'token';
        this.USER_KEY = 'user';
    }

    getToken() {
        return localStorage.getItem(this.TOKEN_KEY);
    }

    setToken(token) {
        localStorage.setItem(this.TOKEN_KEY, token);
    }

    removeToken() {
        localStorage.removeItem(this.TOKEN_KEY);
        localStorage.removeItem(this.USER_KEY);
    }

    getUser() {
        const userData = localStorage.getItem(this.USER_KEY);
        return userData ? JSON.parse(userData) : null;
    }

    setUser(user) {
        localStorage.setItem(this.USER_KEY, JSON.stringify(user));
    }

    isAuthenticated() {
        return !!this.getToken();
    }

    getUserRole() {
        const user = this.getUser();
        return user ? user.role : null;
    }

    isBibliotecario() {
        return this.getUserRole() === 'Bibliotecario';
    }

    isEstudiante() {
        return this.getUserRole() === 'Estudiante';
    }

    isDocente() {
        return this.getUserRole() === 'Docente';
    }

    logout() {
        this.removeToken();
        window.location.href = '/Home/Login';
    }

    requireAuth(allowedRoles = []) {
        if (!this.isAuthenticated()) {
            window.location.href = '/Home/Login';
            return false;
        }

        if (allowedRoles.length > 0) {
            const userRole = this.getUserRole();
            if (!allowedRoles.includes(userRole)) {
                window.location.href = '/Home/Index';
                return false;
            }
        }

        return true;
    }
}

class ToastService {
    static container = null;

    static init() {
        if (!this.container) {
            this.container = document.createElement('div');
            this.container.className = 'toast-container';
            document.body.appendChild(this.container);
        }
    }

    static show(message, type = 'success') {
        this.init();

        const icons = {
            success: '✓',
            error: '✕',
            warning: '⚠'
        };

        const toast = document.createElement('div');
        toast.className = `toast ${type}`;
        toast.innerHTML = `
            <span class="toast-icon">${icons[type]}</span>
            <span class="toast-message">${message}</span>
            <button class="toast-close" onclick="this.parentElement.remove()">×</button>
        `;

        this.container.appendChild(toast);

        setTimeout(() => {
            toast.style.animation = 'slideIn 0.3s ease reverse';
            setTimeout(() => toast.remove(), 300);
        }, 4000);
    }

    static success(message) {
        this.show(message, 'success');
    }

    static error(message) {
        this.show(message, 'error');
    }

    static warning(message) {
        this.show(message, 'warning');
    }
}

class LoadingService {
    static show(element) {
        if (element) {
            element.innerHTML = '<div class="loading"><div class="spinner"></div></div>';
            element.style.position = 'relative';
        }
    }

    static hide(element) {
        if (element) {
            const spinner = element.querySelector('.loading');
            if (spinner) {
                spinner.remove();
            }
            element.style.position = '';
        }
    }
}

class ModalService {
    static show(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            modal.classList.add('active');
            document.body.style.overflow = 'hidden';
        }
    }

    static hide(modalId) {
        const modal = document.getElementById(modalId);
        if (modal) {
            modal.classList.remove('active');
            document.body.style.overflow = '';
        }
    }

    static init() {
        document.querySelectorAll('.modal-overlay').forEach(modal => {
            modal.addEventListener('click', (e) => {
                if (e.target === modal) {
                    modal.classList.remove('active');
                    document.body.style.overflow = '';
                }
            });
        });

        document.querySelectorAll('.modal-close, [data-close-modal]').forEach(btn => {
            btn.addEventListener('click', () => {
                const modal = btn.closest('.modal-overlay');
                if (modal) {
                    modal.classList.remove('active');
                    document.body.style.overflow = '';
                }
            });
        });
    }
}

class FormService {
    static getData(formElement) {
        const formData = new FormData(formElement);
        const data = {};
        
        for (let [key, value] of formData.entries()) {
            data[key] = value;
        }
        
        return data;
    }

    static reset(formElement) {
        formElement.reset();
    }

    static validate(formElement, rules) {
        let isValid = true;
        const errors = {};

        for (let [field, rule] of Object.entries(rules)) {
            const input = formElement.querySelector(`[name="${field}"]`);
            if (!input) continue;

            const value = input.value.trim();

            if (rule.required && !value) {
                errors[field] = rule.messages?.required || 'Este campo es requerido';
                isValid = false;
            }

            if (rule.minLength && value.length < rule.minLength) {
                errors[field] = rule.messages?.minLength || `Mínimo ${rule.minLength} caracteres`;
                isValid = false;
            }

            if (rule.pattern && !rule.pattern.test(value)) {
                errors[field] = rule.messages?.pattern || 'Formato inválido';
                isValid = false;
            }

            if (rule.email && !this.isValidEmail(value)) {
                errors[field] = rule.messages?.email || 'Email inválido';
                isValid = false;
            }
        }

        return { isValid, errors };
    }

    static isValidEmail(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    static showErrors(formElement, errors) {
        this.clearErrors(formElement);

        for (let [field, message] of Object.entries(errors)) {
            const input = formElement.querySelector(`[name="${field}"]`);
            if (input) {
                input.style.borderColor = '#ef4444';
                
                const errorSpan = document.createElement('span');
                errorSpan.className = 'field-error';
                errorSpan.style.color = '#ef4444';
                errorSpan.style.fontSize = '12px';
                errorSpan.style.marginTop = '4px';
                errorSpan.textContent = message;
                
                input.parentElement.appendChild(errorSpan);
            }
        }
    }

    static clearErrors(formElement) {
        formElement.querySelectorAll('.field-error').forEach(el => el.remove());
        formElement.querySelectorAll('input').forEach(input => {
            input.style.borderColor = '';
        });
    }
}

const authService = new AuthService();
