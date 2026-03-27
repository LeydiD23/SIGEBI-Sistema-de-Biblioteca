using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGA.Persistence;
using SGA.Web.Models;
using System.Diagnostics;

namespace SGA.Web.Controllers
{
    public class HomeController : Controller

    {
        private readonly AppDbContext _Context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _Context = context;
        }

        public IActionResult Index()
        {
            bool conectado = _Context.Database.CanConnect();

            // Puedes mostrar el resultado en la vista o directamente como texto
            ViewBag.Conexion = conectado ? "Conexi�n exitosa" : "No se pudo conectar";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Categorias()
        {
            return View();
        }

        public IActionResult Autores()
        {
            return View();
        }

        public IActionResult MasPrestados()
        {
            return View();
        }

        public IActionResult NuevosIngresos()
        {
            return View();
        }

        public IActionResult MejorValorados()
        {
            return View();
        }

        public IActionResult MisPrestamos()
        {
            return View();
        }

        public IActionResult MisReservas()
        {
            return View();
        }

        public IActionResult Favoritos()
        {
            return View();
        }

        public IActionResult GestionLibros()
        {
            return View();
        }

        public IActionResult GestionUsuarios()
        {
            return View();
        }

        public IActionResult Reportes()
        {
            return View();
        }

        public IActionResult Libros()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
