const API_BASE_URL = 'https://localhost:5001/api';

class ApiClient {
    constructor() {
        this.baseUrl = API_BASE_URL;
    }

    getHeaders() {
        const headers = {
            'Content-Type': 'application/json'
        };

        const token = localStorage.getItem('token');
        if (token) {
            headers['Authorization'] = `Bearer ${token}`;
        }

        return headers;
    }

    async request(endpoint, options = {}) {
        const url = `${this.baseUrl}${endpoint}`;
        
        const config = {
            ...options,
            headers: {
                ...this.getHeaders(),
                ...options.headers
            }
        };

        try {
            const response = await fetch(url, config);
            
            if (!response.ok) {
                const error = await response.json().catch(() => ({ message: 'Error desconocido' }));
                throw new Error(error.message || `HTTP ${response.status}`);
            }

            if (response.status === 204) {
                return null;
            }

            return await response.json();
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    }

    get(endpoint) {
        return this.request(endpoint, { method: 'GET' });
    }

    post(endpoint, data) {
        return this.request(endpoint, {
            method: 'POST',
            body: JSON.stringify(data)
        });
    }

    put(endpoint, data) {
        return this.request(endpoint, {
            method: 'PUT',
            body: JSON.stringify(data)
        });
    }

    delete(endpoint) {
        return this.request(endpoint, { method: 'DELETE' });
    }
}

class LibroService {
    constructor(api) {
        this.api = api;
    }

    async getAll() {
        return this.api.get('/libros');
    }

    async getById(id) {
        return this.api.get(`/libros/${id}`);
    }

    async search(term) {
        return this.api.get(`/libros/search?searchTerm=${encodeURIComponent(term)}`);
    }

    async create(data) {
        return this.api.post('/libros', data);
    }

    async update(id, data) {
        return this.api.put(`/libros/${id}`, data);
    }

    async delete(id) {
        return this.api.delete(`/libros/${id}`);
    }
}

class EstudianteService {
    constructor(api) {
        this.api = api;
    }

    async getAll() {
        return this.api.get('/estudiantes');
    }

    async getById(id) {
        return this.api.get(`/estudiantes/${id}`);
    }

    async getByMatricula(matricula) {
        return this.api.get(`/estudiantes/matricula/${encodeURIComponent(matricula)}`);
    }

    async create(data) {
        return this.api.post('/estudiantes', data);
    }

    async update(id, data) {
        return this.api.put(`/estudiantes/${id}`, data);
    }

    async delete(id) {
        return this.api.delete(`/estudiantes/${id}`);
    }
}

class DocenteService {
    constructor(api) {
        this.api = api;
    }

    async getAll() {
        return this.api.get('/docentes');
    }

    async getById(id) {
        return this.api.get(`/docentes/${id}`);
    }

    async getByCedula(cedula) {
        return this.api.get(`/docentes/cedula/${encodeURIComponent(cedula)}`);
    }

    async create(data) {
        return this.api.post('/docentes', data);
    }

    async update(id, data) {
        return this.api.put(`/docentes/${id}`, data);
    }

    async delete(id) {
        return this.api.delete(`/docentes/${id}`);
    }
}

class PrestamoService {
    constructor(api) {
        this.api = api;
    }

    async getAll() {
        return this.api.get('/prestamos');
    }

    async getById(id) {
        return this.api.get(`/prestamos/${id}`);
    }

    async getByUsuario(estudianteId, docenteId) {
        const params = new URLSearchParams();
        if (estudianteId) params.append('estudianteId', estudianteId);
        if (docenteId) params.append('docenteId', docenteId);
        return this.api.get(`/prestamos/usuario?${params.toString()}`);
    }

    async getVencidos() {
        return this.api.get('/prestamos/vencidos');
    }

    async create(data) {
        return this.api.post('/prestamos', data);
    }

    async update(id, data) {
        return this.api.put(`/prestamos/${id}`, data);
    }

    async devolver(id) {
        return this.api.put(`/prestamos/devolver/${id}`);
    }

    async delete(id) {
        return this.api.delete(`/prestamos/${id}`);
    }
}

class ReservaService {
    constructor(api) {
        this.api = api;
    }

    async getAll() {
        return this.api.get('/reservas');
    }

    async getById(id) {
        return this.api.get(`/reservas/${id}`);
    }

    async getByUsuario(estudianteId, docenteId) {
        const params = new URLSearchParams();
        if (estudianteId) params.append('estudianteId', estudianteId);
        if (docenteId) params.append('docenteId', docenteId);
        return this.api.get(`/reservas/usuario?${params.toString()}`);
    }

    async getByLibro(libroId) {
        return this.api.get(`/reservas/libro/${libroId}`);
    }

    async create(data) {
        return this.api.post('/reservas', data);
    }

    async update(id, data) {
        return this.api.put(`/reservas/${id}`, data);
    }

    async cancelar(id) {
        return this.api.put(`/reservas/cancelar/${id}`);
    }

    async delete(id) {
        return this.api.delete(`/reservas/${id}`);
    }
}

class PenalizacionService {
    constructor(api) {
        this.api = api;
    }

    async getAll() {
        return this.api.get('/penalizaciones');
    }

    async getById(id) {
        return this.api.get(`/penalizaciones/${id}`);
    }

    async getByUsuario(estudianteId, docenteId) {
        const params = new URLSearchParams();
        if (estudianteId) params.append('estudianteId', estudianteId);
        if (docenteId) params.append('docenteId', docenteId);
        return this.api.get(`/penalizaciones/usuario?${params.toString()}`);
    }

    async getActivas(estudianteId, docenteId) {
        const params = new URLSearchParams();
        if (estudianteId) params.append('estudianteId', estudianteId);
        if (docenteId) params.append('docenteId', docenteId);
        return this.api.get(`/penalizaciones/activas?${params.toString()}`);
    }

    async create(data) {
        return this.api.post('/penalizaciones', data);
    }

    async update(id, data) {
        return this.api.put(`/penalizaciones/${id}`, data);
    }

    async pagar(id) {
        return this.api.put(`/penalizaciones/pagar/${id}`);
    }

    async delete(id) {
        return this.api.delete(`/penalizaciones/${id}`);
    }
}

class CategoriaService {
    constructor(api) {
        this.api = api;
    }

    async getAll() {
        return this.api.get('/categorias');
    }

    async getById(id) {
        return this.api.get(`/categorias/${id}`);
    }

    async create(data) {
        return this.api.post('/categorias', data);
    }

    async update(id, data) {
        return this.api.put(`/categorias/${id}`, data);
    }

    async delete(id) {
        return this.api.delete(`/categorias/${id}`);
    }
}

class BibliotecarioService {
    constructor(api) {
        this.api = api;
    }

    async getAll() {
        return this.api.get('/bibliotecarios');
    }

    async getById(id) {
        return this.api.get(`/bibliotecarios/${id}`);
    }

    async create(data) {
        return this.api.post('/bibliotecarios', data);
    }

    async update(id, data) {
        return this.api.put(`/bibliotecarios/${id}`, data);
    }

    async delete(id) {
        return this.api.delete(`/bibliotecarios/${id}`);
    }
}

const api = new ApiClient();
const libroService = new LibroService(api);
const estudianteService = new EstudianteService(api);
const docenteService = new DocenteService(api);
const prestamoService = new PrestamoService(api);
const reservaService = new ReservaService(api);
const penalizacionService = new PenalizacionService(api);
const categoriaService = new CategoriaService(api);
const bibliotecarioService = new BibliotecarioService(api);
