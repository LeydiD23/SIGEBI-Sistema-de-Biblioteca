using SGA.Domain.Enums;

namespace SGA.Domain.Interfaces
{
    public interface IReglasNegocio
    {
        bool PuedePrestar(bool libroDisponible, bool tienePenalizaciones, int prestamosActivos, int limitePrestamos);
        bool PuedeReservar(bool libroDisponible, bool tieneReservaActiva, bool tienePenalizaciones);
        bool PuedeRenovar(DateTime fechaLimite, int renovacionesActuales, int maxRenovaciones);
        decimal CalcularPenalizacion(int diasRetraso, decimal montoPorDia);
        int CalcularDiasRetraso(DateTime fechaLimite);
        TimeSpan CalcularTiempoRestante(DateTime fechaLimite);
        int ObtenerDiasPrestamo(Rol rol);
        DateTime CalcularFechaLimite(Rol rol);
    }
}
