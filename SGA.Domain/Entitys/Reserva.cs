using SGA.Domain.Enums;
using System;

namespace SGA.Domain.Entitys
{
    public class Reserva
    {
        public const int DiasValidezReserva = 3;
        public const int DiasExpiracionMaxima = 7;

        public int Id { get; set; }

        public DateTime FechaReserva { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public int PosicionCola { get; set; }
        public EstadoReserva Estado { get; set; }

        public int LibroId { get; set; }
        public Libro? Libro { get; set; }

        public int? EstudianteId { get; set; }
        public Estudiante? Estudiante { get; set; }

        public int? DocenteId { get; set; }
        public Docente? Docente { get; set; }

        public bool EstaActiva() => Estado == EstadoReserva.Pendiente;

        public bool EstaExpirada()
        {
            if (Estado != EstadoReserva.Pendiente)
                return false;
            return DateTime.Now > FechaExpiracion;
        }

        public bool PuedeCancelarse()
        {
            return Estado == EstadoReserva.Pendiente;
        }

        public (bool exito, string mensaje) Cancelar()
        {
            if (!PuedeCancelarse())
                return (false, "Esta reserva no puede ser cancelada.");

            Estado = EstadoReserva.Cancelada;
            return (true, "Reserva cancelada exitosamente.");
        }

        public (bool exito, string mensaje) Completar()
        {
            if (Estado != EstadoReserva.Pendiente)
                return (false, "Solo se pueden completar reservas pendientes.");

            if (EstaExpirada())
            {
                Estado = EstadoReserva.Expirada;
                return (false, "La reserva ha expirado y no puede completarse.");
            }

            Estado = EstadoReserva.Completada;
            return (true, "Reserva completada exitosamente.");
        }

        public int CalcularDiasRestantes()
        {
            if (Estado != EstadoReserva.Pendiente)
                return 0;

            if (FechaExpiracion < DateTime.Now)
                return 0;

            return (FechaExpiracion - DateTime.Now).Days;
        }

        public TimeSpan ObtenerTiempoRestante()
        {
            if (Estado != EstadoReserva.Pendiente)
                return TimeSpan.Zero;

            if (FechaExpiracion < DateTime.Now)
                return TimeSpan.Zero;

            return FechaExpiracion - DateTime.Now;
        }

        public void MarcarExpirada()
        {
            if (Estado == EstadoReserva.Pendiente && EstaExpirada())
            {
                Estado = EstadoReserva.Expirada;
            }
        }

        public void Crear(int posicionCola = 1)
        {
            FechaReserva = DateTime.Now;
            FechaExpiracion = DateTime.Now.AddDays(DiasValidezReserva);
            PosicionCola = posicionCola;
            Estado = EstadoReserva.Pendiente;
        }

        public string ObtenerEstadoActual()
        {
            return Estado switch
            {
                EstadoReserva.Pendiente => $"Pendiente - Posición #{PosicionCola} - {CalcularDiasRestantes()} días restantes",
                EstadoReserva.Completada => "Completada",
                EstadoReserva.Cancelada => "Cancelada",
                EstadoReserva.Expirada => "Expirada",
                _ => Estado.ToString()
            };
        }

        public string ObtenerNombreUsuario()
        {
            if (Estudiante != null)
                return Estudiante.Nombre;
            if (Docente != null)
                return Docente.Nombre;
            return "Desconocido";
        }
    }
}
