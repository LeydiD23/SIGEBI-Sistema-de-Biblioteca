using SGA.Domain.Enums;
using SGA.Domain.Interfaces;

namespace SGA.Application.Services
{
    public class ReglasNegocioService : IReglasNegocio
    {
        public const int DiasPrestamoEstudiante = 7;
        public const int DiasPrestamoDocente = 14;
        public const int DiasPrestamoBibliotecario = 21;
        public const decimal MontoPenalizacionPorDia = 5.00m;

        public bool PuedePrestar(bool libroDisponible, bool tienePenalizaciones, int prestamosActivos, int limitePrestamos)
        {
            if (!libroDisponible)
                return false;

            if (tienePenalizaciones)
                return false;

            if (prestamosActivos >= limitePrestamos)
                return false;

            return true;
        }

        public bool PuedeReservar(bool libroDisponible, bool tieneReservaActiva, bool tienePenalizaciones)
        {
            if (!libroDisponible)
                return false;

            if (tieneReservaActiva)
                return false;

            if (tienePenalizaciones)
                return false;

            return true;
        }

        public bool PuedeRenovar(DateTime fechaLimite, int renovacionesActuales, int maxRenovaciones)
        {
            if (renovacionesActuales >= maxRenovaciones)
                return false;

            if (fechaLimite < DateTime.Now)
                return false;

            return true;
        }

        public decimal CalcularPenalizacion(int diasRetraso, decimal montoPorDia = MontoPenalizacionPorDia)
        {
            if (diasRetraso <= 0)
                return 0;

            decimal monto = diasRetraso * montoPorDia;

            const decimal montoMinimo = 10.00m;
            const decimal montoMaximo = 500.00m;

            if (monto < montoMinimo)
                monto = montoMinimo;

            if (monto > montoMaximo)
                monto = montoMaximo;

            return Math.Round(monto, 2);
        }

        public int CalcularDiasRetraso(DateTime fechaLimite)
        {
            if (fechaLimite >= DateTime.Now)
                return 0;

            return (DateTime.Now - fechaLimite).Days;
        }

        public TimeSpan CalcularTiempoRestante(DateTime fechaLimite)
        {
            if (fechaLimite <= DateTime.Now)
                return TimeSpan.Zero;

            return fechaLimite - DateTime.Now;
        }

        public int ObtenerDiasPrestamo(Rol rol)
        {
            return rol switch
            {
                Rol.Estudiante => DiasPrestamoEstudiante,
                Rol.Docente => DiasPrestamoDocente,
                Rol.Bibliotecario => DiasPrestamoBibliotecario,
                _ => DiasPrestamoEstudiante
            };
        }

        public DateTime CalcularFechaLimite(Rol rol)
        {
            return DateTime.Now.AddDays(ObtenerDiasPrestamo(rol));
        }
    }
}
