class DashboardManager {
    constructor() {
        this.libros = [];
        this.prestamos = [];
        this.reservas = [];
        this.penalizaciones = [];
        this.init();
    }

    async init() {
        await this.loadData();
        this.renderColecciones();
        this.renderLibrosPopulares();
        this.renderAutoresDestacados();
        this.renderLibroDestacado();
        this.renderStats();
        this.renderProximosPrestamos();
        this.renderLibrosRecientes();
        this.setupEventListeners();
    }

    async loadData() {
        try {
            LoadingService.show(document.querySelector('.dashboard-container'));
            
            const [librosData, prestamosData, reservasData, penalizacionesData] = await Promise.all([
                libroService.getAll().catch(() => []),
                prestamoService.getAll().catch(() => []),
                reservaService.getAll().catch(() => []),
                penalizacionService.getAll().catch(() => [])
            ]);

            this.libros = librosData || [];
            this.prestamos = prestamosData || [];
            this.reservas = reservasData || [];
            this.penalizaciones = penalizacionesData || [];
        } catch (error) {
            console.error('Error loading data:', error);
            
            this.libros = this.getLibrosMock();
            this.prestamos = this.getPrestamosMock();
            this.reservas = [];
            this.penalizaciones = [];
        } finally {
            LoadingService.hide(document.querySelector('.dashboard-container'));
        }
    }

    getLibrosMock() {
        return [
            { id: 1, titulo: 'Cien Anos de Soledad', autor: 'Gabriel Garcia Marquez', estado: 1 },
            { id: 2, titulo: 'Don Quijote de la Mancha', autor: 'Miguel de Cervantes', estado: 2 },
            { id: 3, titulo: 'La Odisea', autor: 'Homero', estado: 1 },
            { id: 4, titulo: '1984', autor: 'George Orwell', estado: 1 },
            { id: 5, titulo: 'El Principito', autor: 'Antoine de Saint-Exupery', estado: 2 }
        ];
    }

    getPrestamosMock() {
        const hoy = new Date();
        const manana = new Date(hoy);
        manana.setDate(manana.getDate() + 1);
        const tresDias = new Date(hoy);
        tresDias.setDate(tresDias.getDate() + 3);
        const sieteDias = new Date(hoy);
        sieteDias.setDate(sieteDias.getDate() + 7);

        return [
            { id: 1, libro: { titulo: 'Cien Anos de Soledad' }, usuario: 'Maria Gomez', fechaLimite: manana.toISOString(), diasRetraso: 0 },
            { id: 2, libro: { titulo: 'Don Quijote' }, usuario: 'Juan Perez', fechaLimite: tresDias.toISOString(), diasRetraso: 0 },
            { id: 3, libro: { titulo: 'La Odisea' }, usuario: 'Ana Lopez', fechaLimite: sieteDias.toISOString(), diasRetraso: 0 },
            { id: 4, libro: { titulo: '1984' }, usuario: 'Carlos Ruiz', fechaLimite: hoy.toISOString(), diasRetraso: 0 }
        ];
    }

    getCategoriasMock() {
        return [
            { id: 1, nombre: 'Clasicos' },
            { id: 2, nombre: 'Ciencia Ficcion' },
            { id: 3, nombre: 'Desarrollo Personal' },
            { id: 4, nombre: 'Terror' }
        ];
    }

    renderStats() {
        const librosDisponibles = this.libros.filter(l => l.estado === 1).length;
        const prestamosActivos = this.prestamos.filter(p => p.estado === 1).length;
        const reservasPendientes = this.reservas.filter(r => r.estado === 1).length;
        const multasActivas = this.penalizaciones.filter(p => p.estado === 1).length;
        const devueltosMes = this.prestamos.filter(p => p.estado === 2).length;
        const vencidosMes = this.prestamos.filter(p => p.estado === 3).length;

        document.getElementById('statLibros').textContent = librosDisponibles || 0;
        document.getElementById('statPrestamos').textContent = prestamosActivos || 0;
        document.getElementById('statReservas').textContent = reservasPendientes || 0;
        document.getElementById('statMultas').textContent = multasActivas || 0;
        document.getElementById('statDevueltos').textContent = devueltosMes || 0;
        document.getElementById('statVencidos').textContent = vencidosMes || 0;
    }

    renderProximosPrestamos() {
        const container = document.getElementById('proximosPrestamos');
        if (!container) return;

        const hoy = new Date();
        const tresDiasDespues = new Date(hoy);
        tresDiasDespues.setDate(tresDiasDespues.getDate() + 3);

        const proximos = this.prestamos
            .filter(p => {
                if (p.estado !== 1) return false;
                if (!p.fechaLimite) return false;
                const fechaLimite = new Date(p.fechaLimite);
                return fechaLimite >= hoy && fechaLimite <= tresDiasDespues;
            })
            .slice(0, 5);

        if (proximos.length === 0) {
            container.innerHTML = `
                <div class="empty-state">
                    <div class="empty-state-icon">OK</div>
                    <p class="empty-state-text">No hay prestamos proximos a vencer</p>
                </div>
            `;
            return;
        }

        container.innerHTML = proximos.map(p => {
            const fechaLimite = new Date(p.fechaLimite);
            const diasRestantes = Math.ceil((fechaLimite - hoy) / (1000 * 60 * 60 * 24));
            const esUrgente = diasRestantes <= 1;
            
            return `
                <div class="activity-item ${esUrgente ? 'urgent' : ''}">
                    <div class="activity-icon">
                        <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2">
                            <path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20"></path>
                            <path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z"></path>
                        </svg>
                    </div>
                    <div class="activity-content">
                        <div class="activity-text">${p.libro?.titulo || 'Libro'}</div>
                        <div class="activity-time ${esUrgente ? 'urgent' : ''}">
                            ${p.usuario || 'Usuario'} - Vence en ${diasRestantes} dia(s)
                        </div>
                    </div>
                </div>
            `;
        }).join('');
    }

    renderLibrosRecientes() {
        const container = document.getElementById('librosRecientes');
        if (!container) return;

        const recientes = this.libros.slice(0, 4);

        if (recientes.length === 0) {
            container.innerHTML = `
                <div class="empty-state">
                    <div class="empty-state-icon">OK</div>
                    <p class="empty-state-text">No hay libros registrados</p>
                </div>
            `;
            return;
        }

        container.innerHTML = recientes.map(libro => `
            <div class="recent-book-item" onclick="window.location.href='/Home/Libros/${libro.id}'">
                <div class="recent-book-cover"></div>
                <div class="recent-book-title">${libro.titulo || 'Sin titulo'}</div>
                <div class="recent-book-author">${libro.autor || 'Autor desconocido'}</div>
            </div>
        `).join('');
    }

    renderColecciones() {
        const collectionsGrid = document.querySelector('.collections-grid');
        if (!collectionsGrid) return;

        const collections = [
            { nombre: 'Clasicos', clase: 'clasicos' },
            { nombre: 'Ciencia Ficcion', clase: 'ficcion' },
            { nombre: 'Desarrollo Personal', clase: 'desarrollo' },
            { nombre: 'Terror', clase: 'terror' }
        ];

        collectionsGrid.innerHTML = collections.map(c => `
            <div class="collection-card ${c.clase}" data-categoria="${c.nombre}">
                <div class="collection-icon"></div>
                <div class="collection-title">${c.nombre}</div>
            </div>
        `).join('');

        collectionsGrid.querySelectorAll('.collection-card').forEach(card => {
            card.addEventListener('click', () => {
                const categoria = card.dataset.categoria;
                window.location.href = `/Home/Libros?categoria=${encodeURIComponent(categoria)}`;
            });
        });
    }

    renderLibrosPopulares() {
        const booksList = document.querySelector('.books-list');
        if (!booksList) return;

        const popularBooks = this.libros.slice(0, 5);

        booksList.innerHTML = popularBooks.map((libro, index) => `
            <div class="book-item" data-id="${libro.id}">
                <span class="book-rank ${index < 3 ? 'top' : ''}">${index + 1}</span>
                <div class="book-cover"></div>
                <div class="book-info">
                    <div class="book-title">${libro.titulo || 'Sin titulo'}</div>
                    <div class="book-author">${libro.autor || 'Autor desconocido'}</div>
                    <div class="book-meta">
                        <span class="book-status ${libro.estado === 1 ? 'available' : 'borrowed'}">
                            ${this.getEstadoTexto(libro.estado)}
                        </span>
                    </div>
                </div>
                <button class="book-btn" onclick="dashboard.reservarLibro(${libro.id})">
                    Reservar
                </button>
            </div>
        `).join('');

        booksList.querySelectorAll('.book-item').forEach(item => {
            item.addEventListener('click', (e) => {
                if (!e.target.classList.contains('book-btn')) {
                    const id = item.dataset.id;
                    window.location.href = `/Home/Libros/${id}`;
                }
            });
        });
    }

    getEstadoTexto(estado) {
        const estados = {
            1: 'Disponible',
            2: 'Prestado',
            3: 'Reservado',
            4: 'En Reparacion',
            5: 'Dado de Baja'
        };
        return estados[estado] || 'Disponible';
    }

    renderAutoresDestacados() {
        const authorsList = document.querySelector('.authors-list');
        if (!authorsList) return;

        const autores = this.getAutoresUnicos();

        authorsList.innerHTML = autores.map(autor => `
            <div class="author-item" data-autor="${autor}">
                <div class="author-avatar">${autor.charAt(0)}</div>
                <div class="author-info">
                    <div class="author-name">${autor}</div>
                    <div class="author-books">${this.getLibrosPorAutor(autor)} libro(s)</div>
                </div>
            </div>
        `).join('');

        authorsList.querySelectorAll('.author-item').forEach(item => {
            item.addEventListener('click', () => {
                const autor = item.dataset.autor;
                window.location.href = `/Home/Libros?autor=${encodeURIComponent(autor)}`;
            });
        });
    }

    getAutoresUnicos() {
        const autores = {};
        this.libros.forEach(libro => {
            const autor = libro.autor || 'Desconocido';
            autores[autor] = (autores[autor] || 0) + 1;
        });

        return Object.keys(autores)
            .sort((a, b) => autores[b] - autores[a])
            .slice(0, 5);
    }

    getLibrosPorAutor(autor) {
        return this.libros.filter(l => l.autor === autor).length;
    }

    renderLibroDestacado() {
        const featuredBook = document.querySelector('.featured-book');
        if (!featuredBook) return;

        const libro = this.libros[0] || {
            id: 0,
            titulo: 'Sin libros disponibles',
            autor: ''
        };

        featuredBook.innerHTML = `
            <div class="featured-cover"></div>
            <h3 class="featured-title">${libro.titulo || 'Sin titulo'}</h3>
            <p class="featured-author">${libro.autor || 'Autor desconocido'}</p>
            <div class="featured-actions">
                <button class="featured-btn primary" onclick="dashboard.reservarLibro(${libro.id})">
                    Reservar
                </button>
                <button class="featured-btn secondary" onclick="dashboard.agregarFavorito(${libro.id})">
                    Favorito
                </button>
            </div>
        `;
    }

    setupEventListeners() {
        const searchInput = document.querySelector('.search-input');
        if (searchInput) {
            searchInput.addEventListener('keyup', (e) => {
                if (e.key === 'Enter') {
                    this.buscarLibros(searchInput.value);
                }
            });
        }

        const heroBtn = document.querySelector('.hero-btn');
        if (heroBtn) {
            heroBtn.addEventListener('click', () => {
                window.location.href = '/Home/Libros';
            });
        }

        ModalService.init();
    }

    async buscarLibros(term) {
        if (!term.trim()) {
            ToastService.warning('Ingresa un termino de busqueda');
            return;
        }

        try {
            const resultados = await libroService.search(term);
            window.location.href = `/Home/Libros?search=${encodeURIComponent(term)}`;
        } catch (error) {
            ToastService.error('Error en la busqueda');
        }
    }

    async reservarLibro(libroId) {
        ToastService.success(`Funcion de reserva para libro ${libroId} - Por implementar`);
    }

    agregarFavorito(libroId) {
        const favoritos = JSON.parse(localStorage.getItem('favoritos') || '[]');
        
        if (!favoritos.includes(libroId)) {
            favoritos.push(libroId);
            localStorage.setItem('favoritos', JSON.stringify(favoritos));
            ToastService.success('Libro agregado a favoritos');
        } else {
            ToastService.warning('Este libro ya esta en tus favoritos');
        }
    }
}

const dashboard = new DashboardManager();
