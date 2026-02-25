using SGA.Domain.Enums;
using System;

namespace SGA.Domain.Entitys
{
    public class Reserva
    {
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
    }
}
