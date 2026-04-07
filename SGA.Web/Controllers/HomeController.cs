using Microsoft.AspNetCore.Mvc;
using SGA.Web.Services;

namespace SGA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILibroApiService _libroService;
        private readonly ICategoriaApiService _categoriaService;
        private readonly IPrestamoApiService _prestamoService;

        public HomeController(
            ILogger<HomeController> logger, 
            ILibroApiService libroService,
            ICategoriaApiService categoriaService,
            IPrestamoApiService prestamoService)
        {
            _logger = logger;
            _libroService = libroService;
            _categoriaService = categoriaService;
            _prestamoService = prestamoService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var libros = await _libroService.GetAllAsync();
                var categorias = await _categoriaService.GetAllAsync();

                ViewBag.LibrosRecientes = libros?.OrderByDescending(l => l.FechaAdquisicion).Take(6).ToList();
                ViewBag.Categorias = categorias?.Select(c => new { c.Id, c.Nombre, Libros = libros?.Where(l => l.CategoriaId == c.Id).ToList() ?? new List<Application.DTOs.LibroDto>() }).ToList();
                ViewBag.PrestamosActivos = new List<Application.DTOs.PrestamoDto>();
                ViewBag.Conexion = "API conectada";
            }
            catch (Exception ex)
            {
                ViewBag.Conexion = "Error de conexión: " + ex.Message;
                ViewBag.LibrosRecientes = new List<Application.DTOs.LibroDto>();
                ViewBag.Categorias = new List<object>();
                ViewBag.PrestamosActivos = new List<Application.DTOs.PrestamoDto>();
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Categorias()
        {
            try
            {
                var categorias = await _categoriaService.GetAllAsync();
                var libros = await _libroService.GetAllAsync();
                
                var categoriasConLibros = categorias?.Select(c => new
                {
                    c.Id,
                    c.Nombre,
                    Libros = libros?.Where(l => l.CategoriaId == c.Id).ToList() ?? new List<Application.DTOs.LibroDto>()
                }).ToList();
                
                return View(categoriasConLibros);
            }
            catch
            {
                return View(new List<object>());
            }
        }

        public async Task<IActionResult> Autores()
        {
            try
            {
                var libros = await _libroService.GetAllAsync();
                var autores = libros
                    .Where(l => !string.IsNullOrEmpty(l.Autor))
                    .Select(l => l.Autor)
                    .Distinct()
                    .OrderBy(a => a)
                    .Select(a => new
                    {
                        Nombre = a,
                        TotalLibros = libros.Count(l => l.Autor == a)
                    }).ToList();

                ViewBag.Autores = autores;
            }
            catch
            {
                ViewBag.Autores = new List<object>();
            }
            return View();
        }

        public async Task<IActionResult> MasPrestados()
        {
            try
            {
                var prestamos = await _prestamoService.GetAllAsync();
                var libros = await _libroService.GetAllAsync();

                var librosConPrestamos = libros?.Select(l => new
                {
                    Libro = l,
                    TotalPrestamos = prestamos?.Count(p => p.LibroId == l.Id) ?? 0
                })
                .Where(x => x.TotalPrestamos > 0)
                .OrderByDescending(x => x.TotalPrestamos)
                .Take(20)
                .ToList();

                ViewBag.LibrosPrestados = librosConPrestamos;
            }
            catch
            {
                ViewBag.LibrosPrestados = new List<object>();
            }
            return View();
        }

        public async Task<IActionResult> NuevosIngresos()
        {
            try
            {
                var libros = await _libroService.GetAllAsync();
                return View(libros?.OrderByDescending(l => l.FechaAdquisicion).Take(30).ToList() ?? new List<Application.DTOs.LibroDto>());
            }
            catch
            {
                return View(new List<Application.DTOs.LibroDto>());
            }
        }

        public async Task<IActionResult> MejorValorados()
        {
            try
            {
                var prestamos = await _prestamoService.GetAllAsync();
                var libros = await _libroService.GetAllAsync();

                var librosConRating = libros?.Select(l => new
                {
                    Libro = l,
                    TotalPrestamos = prestamos?.Count(p => p.LibroId == l.Id) ?? 0,
                    Rating = prestamos?.Count(p => p.LibroId == l.Id) > 0 ? 4.5 : 0.0
                })
                .OrderByDescending(x => x.TotalPrestamos)
                .Take(20)
                .ToList();

                ViewBag.LibrosConRating = librosConRating;
            }
            catch
            {
                ViewBag.LibrosConRating = new List<object>();
            }
            return View();
        }

        public async Task<IActionResult> MisPrestamos(int? estado)
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            ViewBag.EstadoFiltro = estado;
            ViewBag.IsLoggedIn = usuarioId.HasValue;

            if (!usuarioId.HasValue || string.IsNullOrEmpty(tipoUsuario))
            {
                return View(new List<Application.DTOs.PrestamoDto>());
            }

            try
            {
                int? estudianteId = tipoUsuario == "estudiante" ? usuarioId : null;
                int? docenteId = tipoUsuario == "docente" ? usuarioId : null;

                var prestamos = await _prestamoService.GetByUsuarioIdAsync(estudianteId, docenteId);

                if (estado.HasValue && estado.Value > 0)
                {
                    var estadoEnum = (Domain.Enums.EstadoPrestamo)estado.Value;
                    prestamos = prestamos?.Where(p => p.Estado == estadoEnum.ToString()).ToList();
                }

                return View(prestamos ?? new List<Application.DTOs.PrestamoDto>());
            }
            catch
            {
                return View(new List<Application.DTOs.PrestamoDto>());
            }
        }

        public async Task<IActionResult> MisReservas()
        {
            int? usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            string? tipoUsuario = HttpContext.Session.GetString("TipoUsuario");

            ViewBag.IsLoggedIn = usuarioId.HasValue;

            if (!usuarioId.HasValue || string.IsNullOrEmpty(tipoUsuario))
            {
                return View(new List<Application.DTOs.ReservaDto>());
            }

            try
            {
                int? estudianteId = tipoUsuario == "estudiante" ? usuarioId : null;
                int? docenteId = tipoUsuario == "docente" ? usuarioId : null;

                var reservas = await _prestamoService.GetByUsuarioIdAsync(estudianteId, docenteId);
                return View(new List<Application.DTOs.ReservaDto>());
            }
            catch
            {
                return View(new List<Application.DTOs.ReservaDto>());
            }
        }

        public IActionResult Favoritos()
        {
            return View(new List<Application.DTOs.LibroDto>());
        }

        public async Task<IActionResult> Libros(string search, string categoria, string autor, string disponibilidad)
        {
            try
            {
                List<Application.DTOs.LibroDto>? libros;

                if (!string.IsNullOrEmpty(search))
                {
                    libros = await _libroService.SearchAsync(search);
                }
                else
                {
                    libros = await _libroService.GetAllAsync();
                }

                if (!string.IsNullOrEmpty(categoria))
                {
                    libros = libros?.Where(l => l.CategoriaNombre == categoria).ToList();
                }

                if (!string.IsNullOrEmpty(autor))
                {
                    libros = libros?.Where(l => l.Autor != null && l.Autor.Contains(autor, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                if (disponibilidad == "disponible")
                {
                    libros = libros?.Where(l => l.StockDisponible > 0).ToList();
                }
                else if (disponibilidad == "prestado")
                {
                    libros = libros?.Where(l => l.StockDisponible == 0).ToList();
                }

                ViewBag.SearchTerm = search;
                ViewBag.CategoriaFiltro = categoria;
                ViewBag.AutorFiltro = autor;
                ViewBag.DisponibilidadFiltro = disponibilidad;
                ViewBag.Categorias = await _categoriaService.GetAllAsync() ?? new List<Application.DTOs.CategoriaDto>();

                return View(libros ?? new List<Application.DTOs.LibroDto>());
            }
            catch
            {
                ViewBag.Categorias = new List<Application.DTOs.CategoriaDto>();
                return View(new List<Application.DTOs.LibroDto>());
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
