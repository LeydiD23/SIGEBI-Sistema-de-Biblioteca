using SGA.Domain.Enums;
using System.Collections.Generic;

namespace SGA.Domain.Entitys
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string Editorial { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int StockDisponible { get; set; }
        public EstadoRecurso Estado { get; set; }
        public DateTime FechaAdquisicion { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public ICollection<Reserva>? Reservas { get; set; }
        public ICollection<Prestamo>? Prestamos { get; set; }

        public bool EstaDisponible() => StockDisponible > 0 && Estado == EstadoRecurso.Disponible;

        public bool PuedeSerPrestado() => StockDisponible > 0;

        public bool Prestar()
        {
            if (!PuedeSerPrestado())
                return false;

            StockDisponible--;
            return true;
        }

        public bool Devolver()
        {
            if (StockDisponible >= Stock)
                return false;

            StockDisponible++;
            return true;
        }

        public (bool esValido, string mensaje) ValidarDisponibilidad()
        {
            if (Estado != EstadoRecurso.Disponible)
                return (false, $"El libro no está disponible. Estado actual: {Estado}");

            if (StockDisponible <= 0)
                return (false, "No hay copias disponibles para préstamo");

            return (true, "Libro disponible para préstamo");
        }

        public int ObtenerCantidadReservados() => Reservas?.Count(r => r.Estado == EstadoReserva.Pendiente) ?? 0;

        public int ObtenerCantidadPrestados() => Prestamos?.Count(p => p.Estado == EstadoPrestamo.Activo) ?? 0;

        public bool EstaEnMantenimiento() => Estado == EstadoRecurso.EnReparacion;

        public bool EstaDadoDeBaja() => Estado == EstadoRecurso.DadoDeBaja;

        public string ObtenerInformacion()
        {
            return $"Título: {Titulo}, Autor: {Autor}, Stock: {StockDisponible}/{Stock}";
        }
    }
}
