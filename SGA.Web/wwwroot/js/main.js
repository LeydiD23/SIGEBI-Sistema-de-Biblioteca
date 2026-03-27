document.addEventListener('DOMContentLoaded', function() {
    initializeApp();
});

function initializeApp() {
    initializeSidebar();
    initializeNotifications();
    updateNotificationsCount();
}

function initializeSidebar() {
    const navLinks = document.querySelectorAll('.nav-link');
    const currentPath = window.location.pathname;

    navLinks.forEach(link => {
        const href = link.getAttribute('href');
        
        if (href && currentPath.includes(href.split('/').pop().split('.')[0])) {
            link.classList.add('active');
        }

        link.addEventListener('click', function(e) {
            if (this.getAttribute('href') && !this.getAttribute('href').startsWith('#')) {
                return;
            }

            navLinks.forEach(l => l.classList.remove('active'));
            this.classList.add('active');
        });
    });
}

function initializeNotifications() {
    const notificationBtn = document.querySelector('.header-btn.notifications');
    if (notificationBtn) {
        notificationBtn.addEventListener('click', toggleNotificationsPanel);
    }
}

let notificationsOpen = false;

function toggleNotificationsPanel() {
    notificationsOpen = !notificationsOpen;
    
    let panel = document.querySelector('.notifications-panel');
    
    if (notificationsOpen) {
        if (!panel) {
            panel = createNotificationsPanel();
            document.body.appendChild(panel);
        }
        panel.style.display = 'block';
    } else {
        if (panel) {
            panel.style.display = 'none';
        }
    }
}

function createNotificationsPanel() {
    const panel = document.createElement('div');
    panel.className = 'notifications-panel';
    panel.innerHTML = `
        <div class="notifications-header">
            <h4>Notificaciones</h4>
            <button class="mark-read-btn">Marcar todo como leído</button>
        </div>
        <div class="notifications-list">
            <div class="notification-item unread">
                <div class="notification-icon">📚</div>
                <div class="notification-content">
                    <p>Tu préstamo de "Cien Años de Soledad" vence mañana</p>
                    <span class="notification-time">Hace 2 horas</span>
                </div>
            </div>
            <div class="notification-item unread">
                <div class="notification-icon">✓</div>
                <div class="notification-content">
                    <p>Tu reserva de "Don Quijote" está disponible</p>
                    <span class="notification-time">Hace 5 horas</span>
                </div>
            </div>
            <div class="notification-item">
                <div class="notification-icon">📖</div>
                <div class="notification-content">
                    <p>Nuevos libros agregados a la categoría "Ciencia Ficción"</p>
                    <span class="notification-time">Ayer</span>
                </div>
            </div>
        </div>
    `;

    const style = document.createElement('style');
    style.textContent = `
        .notifications-panel {
            position: fixed;
            top: 70px;
            right: 20px;
            width: 360px;
            max-height: 400px;
            background: white;
            border-radius: 12px;
            box-shadow: 0 10px 40px rgba(0,0,0,0.15);
            z-index: 1000;
            display: none;
            overflow: hidden;
        }
        .notifications-header {
            padding: 16px 20px;
            border-bottom: 1px solid #e2e8f0;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .notifications-header h4 {
            font-size: 16px;
            font-weight: 600;
            color: #1e293b;
            margin: 0;
        }
        .mark-read-btn {
            background: none;
            border: none;
            color: #7c3aed;
            font-size: 13px;
            cursor: pointer;
        }
        .notifications-list {
            max-height: 320px;
            overflow-y: auto;
        }
        .notification-item {
            display: flex;
            gap: 12px;
            padding: 16px 20px;
            border-bottom: 1px solid #f1f5f9;
            cursor: pointer;
            transition: background 0.2s;
        }
        .notification-item:hover {
            background: #f8fafc;
        }
        .notification-item.unread {
            background: #faf5ff;
        }
        .notification-item.unread:hover {
            background: #f3e8ff;
        }
        .notification-icon {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            background: #f1f5f9;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 18px;
            flex-shrink: 0;
        }
        .notification-content {
            flex: 1;
        }
        .notification-content p {
            font-size: 14px;
            color: #1e293b;
            margin: 0 0 4px 0;
            line-height: 1.4;
        }
        .notification-time {
            font-size: 12px;
            color: #94a3b8;
        }
    `;
    document.head.appendChild(style);

    return panel;
}

function updateNotificationsCount() {
    const badge = document.querySelector('.notification-badge');
    if (badge) {
        const count = 2;
        badge.textContent = count;
        badge.style.display = count > 0 ? 'flex' : 'none';
    }
}

function navigateTo(page) {
    window.location.href = `/Home/${page}`;
}

function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    window.location.href = '/Home/Login';
}

function formatDate(dateString) {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES', {
        day: '2-digit',
        month: 'short',
        year: 'numeric'
    });
}

function formatDateTime(dateString) {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('es-ES', {
        day: '2-digit',
        month: 'short',
        year: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function debounce(func, wait) {
    let timeout;
    return function executedFunction(...args) {
        const later = () => {
            clearTimeout(timeout);
            func(...args);
        };
        clearTimeout(timeout);
        timeout = setTimeout(later, wait);
    };
}

window.navigateTo = navigateTo;
window.logout = logout;
window.formatDate = formatDate;
window.formatDateTime = formatDateTime;
window.debounce = debounce;
