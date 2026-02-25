using SGA.Domain.Enums;
using System.Collections.Generic;

namespace SGA.Domain.Entitys
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public string Ubicacion { get; set; }
        public string Editorial { get; set; }
        public int Stock { get; set; }
        public int StockDisponible { get; set; }
        public EstadoRecurso Estado { get; set; }
        public DateTime FechaAdquisicion { get; set; }

        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }

        public ICollection<Reserva>? Reservas { get; set; }
        public ICollection<Prestamo>? Prestamos { get; set; }
    }
}
