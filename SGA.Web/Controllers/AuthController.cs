using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGA.Persistence;
using SGA.Domain.Interfaces;

namespace SGA.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public AuthController(AppDbContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UsuarioId") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string tipoUsuario, string identificador, string password)
        {
            if (string.IsNullOrEmpty(identificador) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Por favor complete todos los campos";
                return View();
            }

            bool loginExitoso = false;
            int usuarioId = 0;
            string nombreUsuario = "";
            string rol = "";

            if (tipoUsuario == "estudiante")
            {
                var estudiante = _context.Estudiantes
                    .FirstOrDefault(e => e.Matricula == identificador && e.Estado);

                if (estudiante != null)
                {
                    if (estudiante.PasswordHash != null && _passwordHasher.VerifyPassword(password, estudiante.PasswordHash))
                    {
                        loginExitoso = true;
                        usuarioId = estudiante.Id;
                        nombreUsuario = estudiante.Nombre;
                        rol = "Estudiante";
                    }
                    else if (estudiante.PasswordHash == null)
                    {
                        if (password == estudiante.Matricula)
                        {
                            loginExitoso = true;
                            usuarioId = estudiante.Id;
                            nombreUsuario = estudiante.Nombre;
                            rol = "Estudiante";
                        }
                    }
                }
            }
            else if (tipoUsuario == "docente")
            {
                var docente = _context.Docentes
                    .FirstOrDefault(d => d.Cedula == identificador && d.Estado);

                if (docente != null)
                {
                    if (docente.PasswordHash != null && _passwordHasher.VerifyPassword(password, docente.PasswordHash))
                    {
                        loginExitoso = true;
                        usuarioId = docente.Id;
                        nombreUsuario = docente.Nombre;
                        rol = "Docente";
                    }
                    else if (docente.PasswordHash == null)
                    {
                        if (password == docente.Cedula)
                        {
                            loginExitoso = true;
                            usuarioId = docente.Id;
                            nombreUsuario = docente.Nombre;
                            rol = "Docente";
                        }
                    }
                }
            }

            if (loginExitoso)
            {
                HttpContext.Session.SetInt32("UsuarioId", usuarioId);
                HttpContext.Session.SetString("NombreUsuario", nombreUsuario);
                HttpContext.Session.SetString("Rol", rol);
                HttpContext.Session.SetString("TipoUsuario", tipoUsuario);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Credenciales incorrectas. Verifique su matrícula/cédula y contraseña.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Success"] = "Sesión cerrada exitosamente";
            return RedirectToAction("Login", "Auth");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
