using SGA.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace SGA.Domain.Entitys
{
    public class Estudiante
    {
        public const int LimitePrestamos = 5;
        public const int LimiteReservas = 3;

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Matricula { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Carrera { get; set; } = string.Empty;
        public bool Estado { get; set; }

        public string? PasswordHash { get; set; }
        public Rol Rol { get; set; } = Rol.Estudiante;

        public ICollection<Reserva>? Reservas { get; set; }
        public ICollection<Prestamo>? Prestamos { get; set; }
        public ICollection<Penalizacion>? Penalizaciones { get; set; }

        public bool EstaActivo() => Estado;

        public void Activar() => Estado = true;

        public void Desactivar() => Estado = false;

        public int ObtenerPrestamosActivos() => 
            Prestamos?.Count(p => p.Estado == EstadoPrestamo.Activo) ?? 0;

        public int ObtenerReservasActivas() => 
            Reservas?.Count(r => r.Estado == EstadoReserva.Pendiente) ?? 0;

        public int ObtenerPrestamosVencidos() => 
            Prestamos?.Count(p => p.Estado == EstadoPrestamo.Vencido || 
                (p.Estado == EstadoPrestamo.Activo && p.FechaLimite < System.DateTime.Now)) ?? 0;

        public bool TienePenalizacionesActivas() => 
            Penalizaciones?.Any(p => p.Estado == EstadoPenalizacion.Activa || p.Estado == EstadoPenalizacion.Pendiente) ?? false;

        public decimal ObtenerTotalPenalizaciones() => 
            Penalizaciones?.Where(p => p.Estado != EstadoPenalizacion.Pagada).Sum(p => p.Monto) ?? 0;

        public bool PuedePrestar()
        {
            if (!Estado)
                return false;

            if (TienePenalizacionesActivas())
                return false;

            if (ObtenerPrestamosActivos() >= LimitePrestamos)
                return false;

            return true;
        }

        public bool PuedeReservar()
        {
            if (!Estado)
                return false;

            if (TienePenalizacionesActivas())
                return false;

            if (ObtenerReservasActivas() >= LimiteReservas)
                return false;

            return true;
        }

        public bool PuedeRenovar(Prestamo prestamo)
        {
            if (!Estado)
                return false;

            if (prestamo.EstudianteId != Id)
                return false;

            if (!prestamo.PuedeRenovarse())
                return false;

            return true;
        }

        public (bool puede, string mensaje) ValidarLimitePrestamos()
        {
            int prestamosActivos = ObtenerPrestamosActivos();

            if (!Estado)
                return (false, "El estudiante está inactivo.");

            if (prestamosActivos >= LimitePrestamos)
                return (false, $"Ha alcanzado el límite de {LimitePrestamos} préstamos activos.");

            if (TienePenalizacionesActivas())
                return (false, "Tiene penalizaciones activas. No puede realizar nuevos préstamos.");

            return (true, $"Puede realizar préstamos. Préstamos activos: {prestamosActivos}/{LimitePrestamos}");
        }

        public (bool puede, string mensaje) ValidarLimiteReservas()
        {
            int reservasActivas = ObtenerReservasActivas();

            if (!Estado)
                return (false, "El estudiante está inactivo.");

            if (reservasActivas >= LimiteReservas)
                return (false, $"Ha alcanzado el límite de {LimiteReservas} reservas activas.");

            if (TienePenalizacionesActivas())
                return (false, "Tiene penalizaciones activas. No puede realizar nuevas reservas.");

            return (true, $"Puede realizar reservas. Reservas activas: {reservasActivas}/{LimiteReservas}");
        }

        public string ObtenerInformacion()
        {
            return $"Estudiante: {Nombre} - Matrícula: {Matricula} - Préstamos: {ObtenerPrestamosActivos()}/{LimitePrestamos}";
        }

        public void EstablecerPassword(string hashedPassword)
        {
            PasswordHash = hashedPassword;
        }

        public bool TienePassword() => !string.IsNullOrEmpty(PasswordHash);
    }
}
